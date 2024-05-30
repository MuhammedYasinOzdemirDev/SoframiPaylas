# Sofranı Paylaş Platformu
![image](https://github.com/MuhammedYasinOzdemirDev/SoframiPaylas/assets/94251353/6890af79-4723-4323-bcfb-73aff0ed974a)

"Sofranı Paylaş" platformunun GitHub deposuna hoş geldiniz. Bu platform, gıda erişim zorlukları çeken bireyleri yemeklerini paylaşmaya gönüllü ev sahipleriyle buluşturan yenilikçi bir sosyal platformdur. Platform, yemek paylaşımı aracılığıyla topluluk bağlarını güçlendirirken gıda israfı ve güvenliğini de ele almayı hedeflemektedir.

## Özellikler

- **Kullanıcı Kaydı ve Profil Yönetimi**: Güvenli kullanıcı kaydı ve profil yönetimi.
- **Yemek Paylaşım İlanları**: Kullanıcıların yemek paylaşım ilanları oluşturması ve yanıt vermesi.
- **Etkinlik Planlama ve Katılımı**: Kullanıcıların etkinlik planlaması ve katılımı.
- **Kullanıcı Güvenliği ve Gizlilik Ayarları**: Kullanıcı bilgilerinin korunması ve gizlilik ayarlarının yönetimi.

## Teknolojiler

### Backend (Sunucu Tarafı)
- **.NET Core**: API ve sunucu tarafı işlemler için. Güçlü, hızlı ve platformlar arası destek sunar.
- **Firebase**: Veritabanı, kimlik doğrulama ve resim depolama işlemleri için kullanılır. Firebase, gerçek zamanlı veri tabanı hizmetleri ve kullanıcı yönetimi sağlar.
- **Azure API Management**: API'lerin yayımlanması, yönetilmesi ve analiz edilmesi için kullanılır. Güvenli ve ölçeklenebilir API ağ geçitleri sağlar.
- **Ali Baba Cloud**: Ek bulut hizmetleri ve veri depolama çözümleri için kullanılır.

### Frontend (Kullanıcı Arayüzü)
- **HTML, CSS, JavaScript**: Kullanıcı arayüzünün temel yapı taşları. Dinamik ve responsive web sayfaları için kullanılır.
- **Bootstrap & jQuery**: Arayüz geliştirmede hız ve tutarlılık için kullanılan frameworkler. Bootstrap, responsive tasarım kolaylıkları sunar, jQuery ise DOM manipülasyonu ve event handling için kullanılır.
- **HttpClient & Ajax**: Sunucu ile asenkron veri alışverişi için kullanılır. HttpClient .NET uygulamalarında RESTful API tüketimi için, Ajax ise JavaScript ile dinamik web sayfaları oluşturmak için tercih edilir.

### Diğer Teknolojiler
- **Docker**: Uygulamanın konteynerize edilmesi ve platformlar arası dağıtımını sağlar. Geliştirme, test ve üretim ortamları arasında tutarlılık sunar.

## Proje Yapısı

1. **Application Layer (Uygulama Katmanı)**
   - **DTOs (Data Transfer Objects)**: Verileri taşımak için kullanılan nesneler.
   - **Services (İş Mantığı)**: İş mantığını içeren hizmet sınıfları.
   - **Interfaces (Hizmet Arayüzleri)**: Hizmetlerin arayüzleri.

2. **Domain Layer (Domain Katmanı)**
   - **Entities (İş Modelleri ve Veritabanı Nesneleri)**: İş modelleri ve veritabanı nesneleri.

3. **Infrastructure Layer (Altyapı Katmanı)**
   - **Repositories and Interfaces (Veri Erişim Katmanı)**: Veri erişim katmanı.

4. **WebAPI Layer (API Katmanı)**
   - **Controllers (API Kontrolcüler)**: API kontrolcüler.
   - **Configurations and Launch Settings (Yapılandırma Dosyaları)**: Yapılandırma dosyaları.

5. **WebUI Layer (Kullanıcı Arayüzü Katmanı)**
   - **Controllers and Views (MVC Yapı Taşları)**: MVC yapı taşları.
   - **External Services (Oturum, Kimlik Doğrulama ve Depolama Hizmetleri)**: Oturum, kimlik doğrulama ve depolama hizmetleri.

## Önemli Dosyalar ve Fonksiyonları

- **AuthService.cs**: Kimlik doğrulama ve kullanıcı kayıt işlemleri için kullanılan hizmet sınıfı.
- **PostService.cs**: Gönderi oluşturma, silme ve güncelleme işlemleri için kullanılan hizmet sınıfı.
- **User.cs**: Kullanıcı veritabanı nesnesi.
- **AuthRepository.cs**: Firebase üzerinden kullanıcı kayıt ve kimlik doğrulama işlemlerini yapar.
- **AuthController.cs**: API üzerinden kullanıcı kayıt, giriş ve şifre değiştirme işlemlerini yönetir.
- **HomeController.cs**: Anasayfa ve gönderi detay sayfalarını yönetir.
- **Index.cshtml**: Anasayfa görünümünü içerir, kullanıcıya özel mesajlar ve son eklenen gönderiler gösterilir.
- **CustomJwtAuthenticationHandler.cs**: JWT token doğrulama işlemleri için kullanılır.
- **Program.cs**: Uygulama hizmetlerini yapılandırır ve çalıştırır.
## Kurulum

Projeyi yerel geliştirme ortamınızda çalıştırmak için aşağıdaki adımları izleyin:

1. **Depoyu Klonlayın**:
   ```bash
   git clone https://github.com/MuhammedYasinOzdemirDev/SoframiPaylas.git
   cd SoframiPaylas
   dotnet restore
   ```
2. **WebApi**
   ```bash
    cd SoframiPaylas.WebAPI
    dotnet restore
    dotnet run
   ```
3. **WebUI**
   ```bash
    cd SoframiPaylas.WebUI
    dotnet restore
    dotnet run
   ```  
