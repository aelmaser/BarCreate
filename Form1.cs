using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarCreate.Data;
using BarCreate.Models;
using BarCreate.Services;
using Microsoft.EntityFrameworkCore;


namespace BarCreate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ApplyStyling();
            SetupGridColumns();

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                await EnsureDatabaseAsync();   // Migrate
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı kurulurken hata: " + ex.Message);
            }

            await YukleGridAsync();
        }

        private async Task BtnKaydetAsync()
        {
            var stokNo = (txtStokNo.Text ?? "").Trim();
            var miktarText = (txtMiktar.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(stokNo))
            {
                MessageBox.Show("StokNo giriniz.");
                return;
            }

            if (!decimal.TryParse(miktarText, NumberStyles.Number, CultureInfo.InvariantCulture, out var islemMiktari))
            {
                if (!decimal.TryParse(miktarText, out islemMiktari))
                {
                    MessageBox.Show("Miktar geçerli bir sayı olmalı.");
                    return;
                }
            }

            if (islemMiktari <= 0)
            {
                MessageBox.Show("Miktar 0’dan büyük olmalı.");
                return;
            }

            using var db = new AppDbContext();
            using var tx = await db.Database.BeginTransactionAsync();

            btnKaydet.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {

                try
                {

                    try { await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Barkod"); }
                    catch
                    {
                        await db.Database.ExecuteSqlRawAsync("DELETE FROM Barkod"); // eğer truncate edemezse diye önlem alındı.
                    }

                    // Stok bilgisi alınıyor
                    var stok = await db.StokKartBilgileri
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.StokNo == stokNo);

                    if (stok is null)
                    {
                        MessageBox.Show($"StokKartBilgi'de '{stokNo}' bulunamadı.");
                        return;
                    }

                    var kasaIci = stok.KasaIciMiktar;
                    var eksiltme = stok.EksiltmeMiktar;

                    var tamKutu = Math.Floor(islemMiktari / kasaIci); //girilen adete ve stok kartındaki kasa içi miktarına göre tam çıkan kutular
                    var kalan = islemMiktari % kasaIci; //tam kasa olamayacak kalan malzemeleri buluyor

                    int etiketAdedi = (int)tamKutu + (kalan > 0 ? 1 : 0); // tam kutuların üzerine kalan varsa onun adetini de ekleyerek adet sayısını belirliyor

                    // Barkod numaralarının üretildiği kısım
                    var barkodNolar = await BarcodeService.GenerateDailySequentialAsync(db, etiketAdedi);
                    int index = 0;

                    // Tam kutular
                    for (int i = 0; i < (int)tamKutu; i++)
                    {
                        var barkod = new Barkod
                        {
                            BarkodNo = barkodNolar[index++],
                            StokNo = stokNo,
                            KasaIciMiktar = kasaIci,
                            EksiltmeMiktar = eksiltme
                        };
                        db.Barkodlar.Add(barkod);
                    }

                    // Kalan
                    if (kalan > 0)
                    {
                        var kalanEksiltme = Math.Min(kalan, eksiltme);

                        var barkodKalan = new Barkod
                        {
                            BarkodNo = barkodNolar[index++],
                            StokNo = stokNo,
                            KasaIciMiktar = kalan,
                            EksiltmeMiktar = kalanEksiltme
                        };
                        db.Barkodlar.Add(barkodKalan);
                    }

                    await db.SaveChangesAsync();
                    await tx.CommitAsync();

                    await YukleGridAsync(stokNo);
                    MessageBox.Show($"{etiketAdedi} adet barkod oluşturuldu ve tabloya eklendi.");

                }
                finally
                {
                    Cursor = Cursors.Default;
                    btnKaydet.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                MessageBox.Show("İşlem sırasında hata oluştu: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void btnKaydet_Click(object sender, EventArgs e)
        {
            await BtnKaydetAsync();
        }

        private static async Task EnsureDatabaseAsync()
        {
            using var db = new AppDbContext();
            await db.Database.MigrateAsync();   // <-- migrationları otomatik olarak uygular, db yoksa oluşturur en başta
        }

        private async Task YukleGridAsync(string? stokNoFilter = null)
        {
            using var db = new AppDbContext();

            IQueryable<Barkod> q = db.Barkodlar.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(stokNoFilter))
                q = q.Where(b => b.StokNo == stokNoFilter);

            var list = await q
                .OrderBy(b => b.BarkodNo)
                .Select(b => new
                {
                    b.BarkodNo,
                    b.StokNo,
                    b.KasaIciMiktar,
                    b.EksiltmeMiktar
                })
                .ToListAsync();

            dgvBarkodlar.DataSource = list;
        }

        private void ApplyStyling()
        {
            // Form genel
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MinimumSize = new System.Drawing.Size(720, 480);
            this.Font = new System.Drawing.Font("Segoe UI", 10.5f);

            // Label’lar biraz vurgulu
            label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5f, System.Drawing.FontStyle.Bold);
            label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5f, System.Drawing.FontStyle.Bold);

            // TextBox genişlikleri ve hizaları
            txtStokNo.Width = 220;
            txtMiktar.Width = 220;

            // Buton
            btnKaydet.Width = 220;
            btnKaydet.Height = 34;
            btnKaydet.FlatStyle = FlatStyle.System; // native görünüm
            this.AcceptButton = btnKaydet;          // Enter ile kaydet

            // Kontrolleri güzel hizala (soldan/yukarıdan aynı boşluklar)
            int left = 24, top = 24, gapY = 40;
            label1.Left = left; label1.Top = top;
            txtStokNo.Left = left + 90; txtStokNo.Top = top - 3;

            label2.Left = left; label2.Top = top + gapY;
            txtMiktar.Left = left + 90; txtMiktar.Top = label2.Top - 3;

            btnKaydet.Left = left + 90; btnKaydet.Top = label2.Top + gapY + 6;

            // DataGridView konum ve esneklik
            dgvBarkodlar.Left = left;
            dgvBarkodlar.Top = btnKaydet.Bottom + 20;
            dgvBarkodlar.Width = this.ClientSize.Width - 2 * left;
            dgvBarkodlar.Height = this.ClientSize.Height - dgvBarkodlar.Top - 24;
            dgvBarkodlar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Grid stil
            dgvBarkodlar.AutoGenerateColumns = false; // başlıkları biz vereceğiz
            dgvBarkodlar.BackgroundColor = System.Drawing.Color.White;
            dgvBarkodlar.BorderStyle = BorderStyle.None;
            dgvBarkodlar.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBarkodlar.EnableHeadersVisualStyles = false;
            dgvBarkodlar.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            dgvBarkodlar.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvBarkodlar.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5f);
            dgvBarkodlar.ColumnHeadersHeight = 34;
            dgvBarkodlar.RowHeadersVisible = false;
            dgvBarkodlar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBarkodlar.MultiSelect = false;
            dgvBarkodlar.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            dgvBarkodlar.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(221, 236, 254);
            dgvBarkodlar.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            // Scroll ve flicker azaltma
            EnableDoubleBuffering(dgvBarkodlar);
        }

        private void SetupGridColumns()
        {
            dgvBarkodlar.Columns.Clear();

            dgvBarkodlar.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BarkodNo",
                HeaderText = "Barkod No",
                FillWeight = 130
            });
            dgvBarkodlar.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StokNo",
                HeaderText = "Stok No",
                FillWeight = 90
            });
            dgvBarkodlar.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "KasaIciMiktar",
                HeaderText = "Kutu/Adet",
                FillWeight = 80,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }
            });
            dgvBarkodlar.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "EksiltmeMiktar",
                HeaderText = "Eksiltme",
                FillWeight = 80,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }
            });
        }

        // DataGridView’de flicker’ı azaltmak için (protected DoubleBuffered alanını açıyoruz)
        private static void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(dgv, true, null);
        }

        // Uyarı göstermek için yardımcı metot
        private async void ShowWarning(string message)
        {
            lblWarning.Text = message;
            lblWarning.Visible = true;
            await Task.Delay(2000); // 2 sn göster
            lblWarning.Visible = false;
        }

        // StokNo giriş kontrolü
        private void txtStokNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // sadece harf (A-Z, a-z) ve kontrol tuşları (backspace vs.) izin ver
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true; // karakteri iptal et
                ShowWarning("Stok Bilgisi sadece harf olabilir!");
                return;
            }
        }

        // StokNo’yu büyük harfe çevir
        private void txtStokNo_TextChanged(object sender, EventArgs e)
        {
            var pos = txtStokNo.SelectionStart;
            txtStokNo.Text = txtStokNo.Text.ToUpper();
            txtStokNo.SelectionStart = pos; // imleci geri koy
        }

        // Miktar giriş kontrolü
        private void txtMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // sadece rakam ve kontrol tuşları izinli
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                ShowWarning("Miktar sadece rakam olabilir!");
                return;
            }

            // 0 ile başlamasın
            if (char.IsDigit(e.KeyChar))
            {
                // Eğer textbox boşsa ve yazılan karakter '0' ise engelle
                if (txtMiktar.Text.Length == 0 && e.KeyChar == '0')
                {
                    e.Handled = true;
                    ShowWarning("Miktar 0 ile başlayamaz!");
                }
            }
        }
    }
}
