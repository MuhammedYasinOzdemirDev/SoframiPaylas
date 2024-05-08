# Sofranı Paylaş Platformu

"Sofranı Paylaş" platformunun GitHub deposuna hoş geldiniz. Bu platform, gıda erişim zorlukları çeken bireyleri yemeklerini paylaşmaya gönüllü ev sahipleriyle buluşturan yenilikçi bir sosyal platformdur. Platform, yemek paylaşımı aracılığıyla topluluk bağlarını güçlendirirken gıda israfı ve güvenliğini de ele almayı hedeflemektedir.

## Özellikler

- **Kullanıcı Kaydı ve Profil Yönetimi**: Güvenli kullanıcı kaydı ve profil yönetimi.
- **Yemek Paylaşım İlanları**: Kullanıcıların yemek paylaşım ilanları oluşturması ve yanıt vermesi.
- **Etkinlik Planlama ve Katılımı**: Kullanıcıların etkinlik planlaması ve katılımı.
- **Kullanıcı Güvenliği ve Gizlilik Ayarları**: Kullanıcı bilgilerinin korunması ve gizlilik ayarlarının yönetimi.

## Teknolojiler

- **.NET Core**: API ve sunucu tarafı işlemleri için. Güçlü, hızlı ve platformlar arası destek sunar.
- **Firebase**: Veritabanı, kimlik doğrulama ve resim depolama işlemleri için kullanılır. Firebase, gerçek zamanlı veri tabanı hizmetleri ve kullanıcı yönetimi sağlar.
- **Azure API Management**: API'lerin yayımlanması, yönetilmesi ve analiz edilmesi için kullanılır. Güvenli ve ölçeklenebilir API ağ geçitleri sağlar.
- **HTML, CSS, JavaScript**: Kullanıcı arayüzünün temel yapı taşları. Dinamik ve responsive web sayfaları için kullanılır.
- **Bootstrap & jQuery**: Arayüz geliştirmede hız ve tutarlılık için kullanılan frameworkler. Bootstrap, responsive tasarım kolaylıkları sunar, jQuery ise DOM manipülasyonu ve event handling için kullanılır.
- **HttpClient & Ajax**: Sunucu ile asenkron veri alışverişi için kullanılır. HttpClient .NET uygulamalarında RESTful API tüketimi için, Ajax ise JavaScript ile dinamik web sayfaları oluşturmak için tercih edilir.
- **Docker**: Uygulamanın konteynerize edilmesi ve platformlar arası dağıtımını sağlar. Geliştirme, test ve üretim ortamları arasında tutarlılık sunar.


## API Dökümantasyonu
![image](https://github.com/MuhammedYasinOzdemirDev/SoframiPaylas/assets/94251353/6c4dcd57-bb78-4114-a391-8ccb3eae5629)

"Sofranı Paylaş" platformu, RESTful API'ler aracılığıyla bir dizi işlev sunar. İşte bazı önemli API endpoint'leri:

### Food
![image](https://github.com/MuhammedYasinOzdemirDev/SoframiPaylas/assets/94251353/6999eee1-e5df-4458-9f2a-545a575e3ccf)
- `GET /api/Food/foods`: Tüm yiyecek öğelerinin listesini döner.
- `POST /api/Food/food`: Sistemde yeni bir yiyecek öğesi oluşturur.
- `PUT /api/Food/food/{foodId}`: Belirtilen yiyecek öğesini günceller.
- `DELETE /api/Food/food/{foodId}`: Belirtilen yiyecek öğesini siler.

### Post
![image](https://github.com/MuhammedYasinOzdemirDev/SoframiPaylas/assets/94251353/167ff2f3-6e6a-4bbe-84be-9659fd2bf70e)
- `GET /api/Post/posts`: Sistemdeki tüm gönderileri listeler.
- `POST /api/Post/post`: Yeni bir gönderi oluşturur.
- `DELETE /api/Post/post/{postId}`: Belirtilen gönderiyi siler.

### User
![image](https://github.com/MuhammedYasinOzdemirDev/SoframiPaylas/assets/94251353/fedd8079-52db-4878-9dcf-8fc66eca3628)

- `GET /api/User/users`: Tüm kullanıcıların listesini döner.
- `POST /api/User/user`: Yeni bir kullanıcı oluşturur.
- `DELETE /api/User/user/{userId}`: Belirtilen kullanıcıyı siler.

## Kurulum

Projeyi yerel geliştirme ortamınızda çalıştırmak için aşağıdaki adımları izleyin:

```bash
git clone https://github.com/MuhammedYasinOzdemirDev/SoframiPaylas.git
cd sofrani-paylas
dotnet restore
dotnet run
```
## Katkıda Bulunma
Katkılarınızı bekliyoruz! Lütfen bu depoyu forklayın ve önerdiğiniz değişikliklerle pull request gönderin. Büyük değişiklikler için lütfen önce neyi değiştirmek istediğinizi tartışmak üzere bir issue açın.

Testleri uygun şekilde güncellemeyi unutmayın.
