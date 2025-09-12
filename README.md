# BarCreate

**WinForms + Entity Framework Core** ile geliştirilmiş basit bir stok ve barkod uygulaması.  

---

## Amaç
- Kullanıcı, hayali bir **Stok Adı** ve bu stoktaki **Miktar** bilgisini girer.  
- Uygulama `StokKartBilgi` tablosundaki kasa içi miktar ve eksiltme kurallarına göre  
  **otomatik barkod numaraları** üretir ve `Barkod` tablosuna yazar.  
- Barkod numarası formatı: `yyMMdd-benzersizyapannumaradizisi` (örn: `2509120001`).  
- Sonuçlar uygulamada tabloda listelenir.

---

## Kullanılan Teknolojiler
- **C# .NET 8.0 (WinForms)**
- **Entity Framework Core 9**
- **MSSQL / LocalDB**
- **Code-First Migrations** (veritabanı ve tablolar otomatik oluşturulur)

---

## Çalıştırma

1. **İndir:**  
   **https://github.com/aelmaser/BarCreate/releases/tag/v1.0.0** sayfasından en güncel `BarCreate.zip` dosyasını indirin.

2. **Çıkar & Çalıştır:**  
   Zip’i açın, `BarCreate.exe` dosyasına çift tıklayın.

3. **Önkoşul:**  
   - Bilgisayarınızda **SQL Server (LocalDB veya Express)** kurulu olmalıdır.  
   - Varsayılan connection string:  
     ```
     Server=(localdb)\MSSQLLocalDB;Database=CaseStokDb;Trusted_Connection=True;TrustServerCertificate=True;
     ```
     İsterseniz `appsettings.Development.json` içinde bağlantı yolu değiştirilebilir.

4. **İlk açılış:**  
   - EF Core `MigrateAsync()` ile veritabanı ve tablolar otomatik oluşturulur.  

---

## Örnek Test Senaryoları

`StokKartBilgi` tablosundaki veriler:

| StokNo | KasaIciMiktar | EksiltmeMiktar |
|--------|---------------|----------------|
| A      | 50            | 10             |
| B      | 20            | 5              |
| C      | 1200          | 300            |

### Senaryo 1
- **Giriş:** StokNo = `A`, Miktar = `505`  
- **Beklenen:**  
  - 10 × 50’lik barkod (EksiltmeMiktar=10)  
  - 1 × 5’lik barkod (EksiltmeMiktar=5)  
  - Toplam **11 barkod**

### Senaryo 2
- **Giriş:** StokNo = `B`, Miktar = `41`  
- **Beklenen:**  
  - 2 × 20’lik barkod (EksiltmeMiktar=5)  
  - 1 × 1’lik barkod (EksiltmeMiktar=1)  
  - Toplam **3 barkod**

### Senaryo 3
- **Giriş:** StokNo = `C`, Miktar = `2400`  
- **Beklenen:**  
  - 2 × 1200’lük barkod (EksiltmeMiktar=300)  
  - Toplam **2 barkod**
 
### Senaryo 4
- **Giriş:** StokNo = `1`, Miktar = `5`  
- **Beklenen:**  
  - Ekranda Stok adı alanında sadece harf yazılması gerektiği belirten uyarı yazısı.

### Senaryo 5
- **Giriş:** StokNo = `A`, Miktar = `x`  
- **Beklenen:**  
  - Ekranda Miktar alanında sadece rakam yazılması gerektiği belirten uyarı yazısı.
 
### Senaryo 6
- **Giriş:** StokNo = `A`, Miktar = `012`  
- **Beklenen:**  
  - Ekranda Miktar alanında 0 ile başlayan sayı yazılmaması gerektiği belirten uyarı yazısı.

---

## Ekran Görüntüleri

Senaryo 1'in ekran çıktısı ;

<img width="821" height="514" alt="image" src="https://github.com/user-attachments/assets/b646ad2b-a992-4376-9690-0061b35578af" />

Senaryo 4, 5 ve 6'nın ekran çıktısı ;

<img width="788" height="183" alt="image" src="https://github.com/user-attachments/assets/f549064e-9b8f-44d3-b262-1b262105f5bd" />

<img width="793" height="184" alt="image" src="https://github.com/user-attachments/assets/0977cde7-e661-4243-9027-f2e37320cd9b" />

<img width="793" height="188" alt="image" src="https://github.com/user-attachments/assets/06615484-1c6f-4c54-996a-4c6c87626cb3" />


