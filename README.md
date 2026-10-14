# Trafik Cezası Yönetim Modülü

Şirket araçlarına gelen trafik cezalarının sisteme girilmesi, çok aşamalı bir onay sürecinden geçirilmesi ve bu süreçte yapılan tüm işlemlerin geçmişiyle birlikte takip edilmesi için geliştirilmiş bir ASP.NET Core MVC uygulaması.

Senaryo olarak binek, çekici, dorse ve kiralık araçlardan oluşan bir filoya sahip bir şirket düşünülmüştür: personel gelen cezayı sisteme girer, yönetici kontrol edip onaylar, finans ödemeyi onaylar ve süreç tamamlanır.

---

## Özellikler

**Araç yönetimi**
- Araç tanımlama, düzenleme ve listeleme
- Araç tipleri: Binek, Çekici, Dorse, Kiralık Araç
- Plaka ve araç tipine göre filtreleme
- Plaka formatı doğrulaması ve tekrarlayan plaka kontrolü (`34 abc 123` → `34ABC123` olarak normalize edilir)

**Onay süreci**
- `Yeni → Yönetici Onayı → Finans Onayı → Tamamlandı` akışı
- Her kullanıcı yalnızca kendi aşamasındaki cezalar üzerinde işlem yapabilir
- Her aşamada onaylama veya ret nedeni belirterek reddetme
- Kullanıcıya özel "Bekleyen İşlerim" ekranı

**Onay geçmişi**
- Ceza üzerinde yapılan her işlemin (oluşturma, onaya gönderme, onay, ret) kaydı
- İşlemi yapan kullanıcı, tarih, işlem tipi, açıklama / ret nedeni, önceki ve yeni durum

**Diğer**
- Ceza türü ve sürücü tanımları
- Rol bazlı kullanıcı yönetimi (oluşturma, aktif / pasif yapma)
- Duruma, araç tipine, tarih aralığına ve tutanak no / plakaya göre filtrelenebilen ceza listesi
- Aşamalara göre ceza sayılarını ve tutarları gösteren ana sayfa

---

## Teknolojiler

- .NET 9, ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server
- Cookie Authentication (ASP.NET Core Identity kullanılmadan)
- Bootstrap 5

---

## Mimari

Proje N-katmanlı mimariyle geliştirilmiştir. Bağımlılıklar her zaman üst katmandan alt katmana doğrudur.

```
TrafficFineManagementModule.sln
├── EntityLayer                      Entity sınıfları ve enum'lar
├── DataAccessLayer                  DbContext, generic repository, EF Core DAL sınıfları, migration'lar, seed
├── BusinessLayer                    İş kuralları, onay akışı (ApprovalWorkflow), servisler
└── TrafficFineManagementModuleWeb   MVC controller'lar, ViewModel'ler, view'lar
```

```
WebUI           → BusinessLayer, DataAccessLayer, EntityLayer
BusinessLayer   → DataAccessLayer, EntityLayer
DataAccessLayer → EntityLayer
```

- **DataAccessLayer:** `IGenericDal<T>` / `GenericRepository<T>` ortak CRUD işlemlerini sağlar. Entity'ye özel sorgular (ör. ilişkili verilerle birlikte ceza listesi) `I...Dal` arayüzlerinde tanımlanıp `Ef...Dal` sınıflarında yazılır.
- **BusinessLayer:** `GenericManager<T>` varsayılan işlemleri sağlar; kural gerektiren manager'lar ilgili metodu `override` eder. Kural ihlallerinde `BusinessException` fırlatılır.
- **WebUI:** Controller'lar doğrudan DAL'a değil, Business servislerine erişir. Formlarda entity yerine ViewModel kullanılır.

Tüm veritabanı işlemleri `async/await` ile asenkron yapılır.

---

## Kurulum

### Gereksinimler

- .NET 9 SDK
- SQL Server (Express veya LocalDB yeterli)
- Visual Studio 2022 veya üzeri

### Adımlar

**1. Depoyu klonlayın**

```bash
git clone <https://github.com/micr0-it/TrafficFineManagementModule>
```

**2. Connection string'i ayarlayın**

`TrafficFineManagementModuleWeb/appsettings.json` dosyasındaki `Server` değerini kendi SQL Server'ınıza göre düzenleyin:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TrafficFineDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

| SQL Server kurulumu | Server değeri |
|---|---|
| Varsayılan instance | `.` |
| SQL Express | `.\\SQLEXPRESS` |
| LocalDB | `(localdb)\\MSSQLLocalDB` |

JSON içinde ters slash iki kez (`\\`) yazılmalıdır. Veritabanını önceden oluşturmanıza gerek yoktur.


Varsayılan değer LocalDB'yi kullanır. Kendi bilgisayarınıza özel bir değeri repoya eklemeden kullanmak için
web projesine sağ tıklayıp **Manage User Secrets** ile açılan `secrets.json` dosyasına aynı
`ConnectionStrings` bölümünü yazabilirsiniz; geliştirme ortamında bu değer `appsettings.json`'daki değeri ezer.


**3. Veritabanını oluşturun**

Visual Studio'da `TrafficFineManagementModuleWeb` projesini startup project yapın. Package Manager Console'da **Default project** olarak `DataAccessLayer` seçip:

```
Update-Database
```

veya komut satırından (`dotnet-ef` aracı kurulu olmalıdır):

```bash
dotnet ef database update --project DataAccessLayer --startup-project TrafficFineManagementModuleWeb
```

**4. Uygulamayı çalıştırın**

F5 ile çalıştırın. İlk açılışta test kullanıcıları ve örnek veriler otomatik olarak oluşturulur.

### Olası sorun: "Uygulama Denetimi ilkesi bu dosyayı engelledi (0x800711C7)"

Windows 11'deki **Akıllı Uygulama Denetimi (Smart App Control)**, derlenen imzasız DLL dosyalarını engelleyebilir. Bu durumda *Windows Güvenliği → Uygulama ve tarayıcı denetimi → Akıllı Uygulama Denetimi* ayarını geliştirme süresince kapatmak gerekir.

---

## Test Kullanıcıları

Tüm kullanıcıların şifresi: **`Test123`**

| Kullanıcı adı | Rol | Yapabildikleri |
|---|---|---|
| `personel` | Personel | Ceza girer, düzenler, onaya gönderir |
| `yonetici` | Yönetici | Yönetici onayı aşamasındaki cezaları onaylar / reddeder |
| `finans` | Finans | Finans onayı aşamasındaki cezaları onaylar / reddeder |
| `admin` | Admin | Araç, ceza türü, sürücü ve kullanıcı tanımları |

---

## Örnek Veri

Uygulama ilk açıldığında, ilgili tablolar boşsa şunlar eklenir:

- 7 ceza türü (biri pasif)
- Her araç tipinden toplam 6 araç
- 4 sürücü
- Onay sürecinin her aşamasından toplam 7 ceza: yeni, yönetici onayında, finans onayında, tamamlanmış (biri sürücüsü bilinmeyen EDS cezası) ve biri yönetici, biri finans tarafından reddedilmiş

Böylece uygulama açılır açılmaz tüm ekranlar ve farklı onay geçmişi senaryoları incelenebilir.

> Ceza tutarları ve puanları **örnektir**, güncel resmi tutarları yansıtmaz. Sürücülerin TC kimlik numaraları gerçek kişilere ait değildir.

Örnek veriler doğrudan veritabanına eklenir (Business katmanı bypass edilir), çünkü geçmiş tarihli ve farklı kullanıcılarca yapılmış işlemler oluşturulması gerekir. Durumlar ve geçmiş kayıtları onay akışı kurallarına uygun şekilde oluşturulur.

---

## Onay Süreci

```
 Yeni ──► Yönetici Onayında ──► Finans Onayında ──► Tamamlandı
              │                       │
              └────────► Reddedildi ◄─┘
```

| Durum | İşlemi yapabilecek rol | Onaylanırsa | Reddedilebilir mi |
|---|---|---|---|
| Yeni | Personel (cezayı giren) | Yönetici Onayında | Hayır (personel düzenleyebilir / silebilir) |
| Yönetici Onayında | Yönetici | Finans Onayında | Evet |
| Finans Onayında | Finans | Tamamlandı | Evet |
| Tamamlandı / Reddedildi | — | — | — |

**Kurallar**
- "Yeni" durumdaki ceza sadece onu giren personel tarafından düzenlenebilir, silinebilir ve onaya gönderilebilir.
- Onaya gönderilmiş ceza düzenlenemez ve silinemez.
- Ret işleminde ret nedeni zorunludur.
- Ceza tutarı formdan alınmaz; ceza türünün o anki tutarından otomatik kopyalanır. Böylece ceza türü tutarı sonradan değişse de eski cezaların tutarı korunur.

**Teknik tasarım**

Akış kuralları kodun içine dağıtılmamış, `BusinessLayer/Workflow/ApprovalWorkflow.cs` içinde tek bir tabloda tanımlanmıştır. Hem iş katmanı (işleme izin verilip verilmeyeceği) hem de arayüz (onay / ret butonlarının gösterilip gösterilmeyeceği) aynı tabloyu kullanır. Yeni bir onay aşaması eklemek için `ApprovalStatus` enum'una bir değer ve bu tabloya bir satır eklemek yeterlidir.

**Admin onay akışında yer almaz**; yalnızca tanımlar ve kullanıcı yönetiminden sorumludur. Bu, görevler ayrılığı (separation of duties) ilkesine uygun olarak bilinçli verilmiş bir karardır.

---

## Onay Geçmişi

Her işlem `TrafficFineApprovalHistories` tablosuna kaydedilir ve cezanın detay sayfasında gösterilir.

| İstenen bilgi | Alan |
|---|---|
| İşlemi gerçekleştiren kullanıcı | `PerformedByUserId` |
| İşlem tarihi | `ActionDate` |
| İşlem tipi | `ActionType` (Oluşturuldu, Onaya Gönderildi, Onaylandı, Reddedildi) |
| Açıklama / ret nedeni | `Description` |
| Önceki durum | `PreviousStatus` (oluşturma işleminde boş) |
| Yeni durum | `NewStatus` |

Cezanın durum değişikliği ile geçmiş kaydı **aynı `SaveChanges` çağrısında**, yani aynı transaction içinde kaydedilir. Durumun değişip geçmişin yazılmaması gibi bir tutarsızlık oluşamaz. Geçmiş kayıtları sadece eklenir; değiştirilmez ve silinmez.

---

## Validation ve Hata Yönetimi

| Hata türü | Nasıl ele alınıyor |
|---|---|
| Form hataları (boş alan, uzunluk, format) | ViewModel'lerde DataAnnotations; hem tarayıcıda hem sunucuda (`ModelState`) kontrol edilir |
| İş kuralı ihlalleri (tekrarlayan plaka, yetkisiz aşama, eksik ret nedeni...) | Business katmanında `BusinessException`; controller'da yakalanıp kullanıcıya anlaşılır mesaj olarak gösterilir |
| Olmayan sayfa / kayıt (404, 400) | `UseStatusCodePagesWithReExecute` ile kullanıcı dostu durum sayfası |
| Beklenmeyen hatalar | `UseExceptionHandler` ile merkezi olarak yakalanır ve loglanır; kullanıcıya teknik detay yerine genel bir mesaj ve hata kodu gösterilir |

Controller'lardaki `try/catch` blokları yalnızca **beklenen** hatalar (`BusinessException`) içindir. Beklenmeyen hatalar her yerde ayrı ayrı yakalanmaz, tek bir merkezden ele alınır.

---


## Güvenlik

| Konu | Önlem |
|---|---|
| SQL Injection | Tüm sorgular EF Core LINQ ile yazılmıştır; değerler parametre olarak gönderilir, elle SQL kullanılmaz. |
| XSS | Razor çıktıları otomatik encode edilir; `Html.Raw` kullanılmaz. |
| CSRF | Veri değiştiren tüm işlemler POST ve anti-forgery token ile yapılır. |
| Yetkilendirme | Varsayılan olarak tüm sayfalar giriş gerektirir; rol kontrolleri controller'da, aşama ve sahiplik kontrolleri ayrıca Business katmanında yapılır. |
| Overposting | Formlarda ViewModel kullanılır; durum, tutar ve kaydı giren kullanıcı sunucuda atanır. |
| Open redirect | Login sonrası sadece yerel adreslere yönlendirilir. |
| Şifreler | `PasswordHasher` (PBKDF2, salt'lı) ile hash'lenerek saklanır. |
| Güvenlik header'ları | `X-Frame-Options`, `X-Content-Type-Options`, `Referrer-Policy`. |

**Bilinen eksikler**
- Hatalı girişlerde hesap kilitleme / istek sınırlama (rate limiting) yoktur.
- Content-Security-Policy eklenmemiştir (view'larda satır içi script'ler bulunduğu için).
- Seed kullanıcıları test amaçlı ortak bir şifre (`Test123`) kullanır; canlı ortama alınmadan önce bu kullanıcılar kaldırılmalı veya şifreleri değiştirilmelidir.

---

## Tasarım Kararları

- **Kimlik doğrulama:** ASP.NET Core Identity yerine Cookie Authentication ve `PasswordHasher<T>` kullanıldı; iki rollü basit bir yapı için daha sade bir veri modeli sağlıyor. Şifreler hash'lenerek saklanır.
- **Varsayılan olarak giriş zorunlu:** `FallbackPolicy` ile tüm sayfalar giriş gerektirir; açık olması gereken sayfalar `[AllowAnonymous]` ile işaretlenir.
- **Pasif kullanıcının oturumu:** Pasif yapılan kullanıcının açık oturumu bir sonraki istekte sonlandırılır (`OnValidatePrincipal`). Bu, her istekte küçük bir veritabanı sorgusu ekler.
- **Silme yerine pasif yapma:** Kullanıcılar ve ceza türleri silinmez, pasif yapılır; böylece geçmiş kayıtları ve eski cezalar bozulmaz. İlişkilerde `DeleteBehavior.Restrict` kullanılmıştır.
- **Enum'lar veritabanında metin olarak** saklanır (`HasConversion<string>`); veritabanında `0`, `1` yerine okunabilir değerler görünür.
- **Ondalık sayılar:** Form verileri kültürden bağımsız (invariant) okunur, ekranda Türkçe biçimde gösterilir. Bu, Türkçe sistemlerde `1234.50` tutarının `123450` olarak okunması sorununu önler.
- **Kişisel veri:** TC kimlik numaraları listelerde maskelenerek gösterilir (`123******01`).
- **Güvenlik:** Veri değiştiren tüm işlemler POST ile ve anti-forgery token ile yapılır; login sonrası yönlendirmede yalnızca yerel adreslere izin verilir.

---

## Geliştirilebilir Noktalar

- Ana sayfadaki sayımlar şu an bellekte hesaplanıyor; kayıt sayısı arttığında veritabanında `GROUP BY` ile yapılmalı.
- Listelerde sayfalama (pagination) eklenebilir.
- Aynı cezaya iki yöneticinin aynı anda işlem yapması durumu için iyimser eşzamanlılık kontrolü (`RowVersion`) eklenebilir.
- Kullanıcıların kendi şifresini değiştirmesi ve admin tarafından şifre sıfırlama.
- Business katmanı için birim testleri.

---

## Commit Kuralları

Commit mesajlarında [Conventional Commits](https://www.conventionalcommits.org/) kalıbı kullanılmıştır:

| Önek | Anlamı |
|---|---|
| `feat:` | Yeni özellik |
| `fix:` | Hata düzeltme |
| `refactor:` | Davranışı değiştirmeyen düzenleme |
| `chore:` | Yapılandırma, migration vb. |
| `docs:` | Dokümantasyon |

---

## Geliştirme Sürecinde Yapay Zekâ Kullanımı

Bu projeyi geliştirirken Claude'u (Anthropic) bir eşli programlama (pair programming) ve danışma aracı olarak kullandım. Kodun nasıl ortaya çıktığı konusunda şeffaf olmak istediğim için bunu açıkça belirtiyorum.

**Yapay zekâdan aldığım destek**
- **Mimari ve tasarım:** N-katmanlı yapının, generic repository kalıbının ve çok aşamalı onay akışının tasarımını yapay zekâ ile tartışarak oluşturdum.
- **Kod önerileri:** Tüm katmanlarda (Entity, DataAccess, Business, WebUI) yapay zekânın önerdiği kodlardan yararlandım. Özellikle arayüz ekranları, ana sayfa ve menü, hata yönetimi, örnek veri (seed) ve bu README'nin hazırlanmasında yoğun destek aldım.
- **Hata ayıklama:** Karşılaştığım derleme ve çalışma zamanı hatalarının sebeplerini yapay zekâ ile birlikte analiz ettim.

**Kendi katkım**
- Ödev gereksinimlerini yorumlayıp kapsamı belirledim. Örneğin ilk tasarımda yer alan ödeme ve itiraz modüllerini ödev kapsamı dışında olduğu için çıkarma, sürücü bilgisini cezada opsiyonel tutma ve ruhsat sahibi ilişkisini kaldırma kararlarını verdim.
- Önerilen kodları İngilizce isimlendirme standardıma ve kendi yazım stilime uyarlayarak projeye ekledim. Anlamadığım noktaları (async/await kullanımı, override mantığı, nullable reference types, shadow foreign key vb.) sorgulayarak ve gerekçelerini öğrenerek ilerledim.
- Tüm özellikleri farklı rollerle elle test ettim. Testler sırasında ortaya çıkan hataları (filtrelerin uygulanmaması, plaka doğrulamasının devreye girmemesi, otomatik oluşturulmuş arayüz taslaklarının çalışma zamanı hatasına yol açması, primary key isimlendirme sorunu vb.) tespit edip düzelttim.
- Git ve GitHub kurulumunu, commit geçmişini, veritabanı ve migration süreçlerini yönettim.

Projenin her bölümünü açıklayabilir ve neden o şekilde tasarlandığını gerekçelendirebilirim.