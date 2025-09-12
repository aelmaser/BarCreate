namespace BarCreate
{
    partial class Form1
    {
         /// <summary>
         ///  Required designer variable.
         /// </summary>
         private System.ComponentModel.IContainer components = null;
         private System.Windows.Forms.Label lblWarning;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// 

        protected override void Dispose(bool disposing)
        {
               if (disposing && (components != null))
               {
                   components.Dispose();
               }
               base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            label1 = new Label();
            txtStokNo = new TextBox();
            label2 = new Label();
            txtMiktar = new TextBox();
            btnKaydet = new Button();
            dgvBarkodlar = new DataGridView();
            lblWarning = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvBarkodlar).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(57, 83);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 0;
            label1.Text = "Stok Adı";
            label1.Click += label1_Click;
            // 
            // txtStokNo
            // 
            txtStokNo.Location = new Point(57, 112);
            txtStokNo.Name = "txtStokNo";
            txtStokNo.Size = new Size(100, 23);
            txtStokNo.TabIndex = 1;
            txtStokNo.TextChanged += txtStokNo_TextChanged;
            txtStokNo.KeyPress += txtStokNo_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(57, 161);
            label2.Name = "label2";
            label2.Size = new Size(41, 15);
            label2.TabIndex = 2;
            label2.Text = "Miktar";
            // 
            // txtMiktar
            // 
            txtMiktar.Location = new Point(57, 188);
            txtMiktar.Name = "txtMiktar";
            txtMiktar.Size = new Size(100, 23);
            txtMiktar.TabIndex = 3;
            txtMiktar.KeyPress += txtMiktar_KeyPress;
            // 
            // btnKaydet
            // 
            btnKaydet.Location = new Point(57, 248);
            btnKaydet.Name = "btnKaydet";
            btnKaydet.Size = new Size(100, 39);
            btnKaydet.TabIndex = 4;
            btnKaydet.Text = "TABLO GETİR";
            btnKaydet.UseVisualStyleBackColor = true;
            btnKaydet.Click += btnKaydet_Click;
            // 
            // dgvBarkodlar
            // 
            dgvBarkodlar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBarkodlar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBarkodlar.Location = new Point(246, 83);
            dgvBarkodlar.Name = "dgvBarkodlar";
            dgvBarkodlar.ReadOnly = true;
            dgvBarkodlar.Size = new Size(494, 303);
            dgvBarkodlar.TabIndex = 5;
            // 
            // lblWarning
            // 
            lblWarning.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblWarning.AutoSize = true;
            lblWarning.BackColor = Color.Transparent;
            lblWarning.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblWarning.ForeColor = Color.Red;
            lblWarning.Location = new Point(438, 28);
            lblWarning.Name = "lblWarning";
            lblWarning.Size = new Size(0, 15);
            lblWarning.TabIndex = 6;
            lblWarning.TextAlign = ContentAlignment.TopRight;
            lblWarning.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvBarkodlar);
            Controls.Add(btnKaydet);
            Controls.Add(txtMiktar);
            Controls.Add(label2);
            Controls.Add(txtStokNo);
            Controls.Add(label1);
            Controls.Add(lblWarning);
            Name = "Form1";
            Text = "BarCreate";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvBarkodlar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label1;
        private TextBox txtStokNo;
        private Label label2;
        private TextBox txtMiktar;
        private Button btnKaydet;
        private DataGridView dgvBarkodlar;
    }
}
