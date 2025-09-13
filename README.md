# BarCreate

**WinForms + Entity Framework Core (release v1.0.2 için)** ile geliştirilmiş basit bir stok ve barkod uygulaması.  

---

## Amaç
- Kullanıcı, bir **Stok Adı** ve bu stoktaki **Miktar** bilgisini girer.  
- Uygulama `StokKartBilgi` tablosundaki kasa içi miktar ve eksiltme kurallarına göre  
  **otomatik barkod numaraları** üretir ve `Barkod` tablosuna yazar.  
- Barkod numarası formatı: `yyMMdd-uniquenumara` (örn: `2509120001`).  
- Sonuçlar uygulamada tabloda listelenir.

---

## Kullanılan Teknolojiler
- **C# .NET 8.0 (WinForms)**
- **Entity Framework Core 9 (release v1.0.2 için)**
- **MSSQL / LocalDB (release v1.0.2 için)**
- **Code-First Migrations (release v1.0.2 için)** (veritabanı ve tablolar otomatik oluşturulur)

---

## Çalıştırma

1. **İndir:**  
   **Veritabanı entegrasyonlu versiyon için : https://github.com/aelmaser/BarCreate/releases/tag/v1.0.2** sayfasından en güncel `BarCreate.zip` dosyasını indirin.
   
   **Veritabanı bağlantısı olmayan in-memory versiyon için : https://github.com/aelmaser/BarCreate/releases/tag/v1.0.3** sayfasından en güncel `BarCreate.zip` dosyasını indirin.

3. **Çıkar & Çalıştır:**  
   Zip’i açın, `BarCreate.exe` dosyasına çift tıklayın.

4. **Önkoşul: (release v1.0.2 için)**  
   - Bilgisayarınızda **SQL Server (LocalDB veya Express)** kurulu olmalıdır.
   - Varsayılan connection string:  
     ```
     Server=(localdb)\MSSQLLocalDB;Database=CaseStokDb;Trusted_Connection=True;TrustServerCertificate=True;
     ```

5. **İlk açılış: (release v1.0.2 için)**  
   - EF Core `MigrateAsync()` ile veritabanı ve tablolar otomatik oluşturulur.  

---

## Hesaplama Yöntemi

1- Barkodlar listesi temizlenirGirilen stok bilgisine göre kasa içi miktarı ve eksiltme bilgisi çekilir. 

2- Kullanıcı tarafından girilen miktar bilgisi öncelikle kasa içi miktarı bilgisine bölünerek tam kasa sayısı ortaya çıkarılır.

3- Kullanıcı tarafından girilen miktar bilgisi kasa içi miktarına göre modu alınarak artık kalan miktar bilgisi ortaya çıkartılır. Eğer bu kalan bilgisi 0'dan büyük ise etiket miktarına bir eklenir.

4- Benzersiz barkodların oluşturulması adına BarcodeService adlı servise etiket adeti bilgisi gönderilerek çalıştırılır. 
   Burada res adlı Liste oluşturulur, prefix adlı değişkene günün yyMMdd formatında bilgisi eklenir, sonrasında bir for döngüsü ile etiket sayısı kadar (prefix + döngü indisinin 4 basamak genişletilmiş hali) bilgisi res listesine her döngüde eklenir.
   
5- Etiket bilgileri oluştuktan sonra tam kutular için stok no, kasa içi miktar ve eksiltme miktarı bilgileriyle DataStore adlı sınıfta tutulan Barkodlar adlı listeye veriler işlenir.

6- Artık kalan bilgi var ise öncelikle eksiltme bilgisi ortaya çıkarılır. Burada artık kalan sayı mı büyük, yoksa stok kart bilgi tablosundaki eksiltme miktarı mı büyük buna bakılır hangisi küçük ise o bilgi eksiltme miktarı olarak kullanılır. Bu bilgiyle birlikte        5.maddedeki gibi tabloda gösterilmek adına stok no, kasa içi miktar ve eksiltme miktarı bilgileriyle DataStore adlı sınıfta tutulan Barkodlar adlı listeye veriler işlenir.

7- YukleGrid yardımcı fonksiyonu ile kullanıcının sonuçları göreceği tabloya bilgiler aktarılır.



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


