# ForumApp

Forum tarzında kullanıcıların paylaşım yapabildiği bir web uygulaması.

## Özellikler

- ✅ Kullanıcı kayıt ve giriş sistemi
- ✅ Rol bazlı yetkilendirme (Admin / Normal Kullanıcı)
- ✅ Post (gönderi) oluşturma, düzenleme, silme
- ✅ Admin paneli ile tüm gönderileri yönetme

## Teknolojiler

- .NET 8 MVC
- ASP.NET Core Identity
- Entity Framework Core
- MS SQL Server
- Bootstrap 5

## Kullanıcı Rolleri

### 👤 Normal Kullanıcı
- Kayıt olup sisteme giriş yapabilir
- Yeni bir post (konu) oluşturabilir
- Kendi oluşturduğu postları güncelleyebilir veya silebilir
- Diğer kullanıcıların postlarını görebilir ancak düzenleyemez

### 🛡 Admin Kullanıcı
- Tüm postları görüntüleyebilir
- Sakıncalı veya uygunsuz postları silebilir

## Kurulum

### Gereksinimler
- .NET 8 SDK
- MS SQL Server veya SQL Server LocalDB

### Adımlar

1. Projeyi klonlayın:
```bash
git clone <repository-url>
cd ForumApp
```

2. appsettings.json dosyasında veritabanı bağlantı dizesini güncelleyin:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ForumAppDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

3. Veritabanı migration'larını çalıştırın:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Uygulamayı çalıştırın:
```bash
dotnet run
```

5. Tarayıcınızda `https://localhost:5001` adresine gidin.

## Varsayılan Admin Hesabı

Uygulama ilk çalıştırıldığında otomatik olarak bir admin hesabı oluşturulur:

- **E-posta:** admin@forumapp.com
- **Şifre:** Admin123!

## Veritabanı Yapısı

### ApplicationUser (AspNetUsers)
- Id (string)
- UserName (string)
- Email (string)
- FirstName (string)
- LastName (string)
- RegisteredDate (DateTime)

### Post
- Id (int)
- Title (string)
- Content (string)
- CreatedDate (DateTime)
- UpdatedDate (DateTime?)
- UserId (string) - Foreign Key

### Roller
- Admin
- User

## Güvenlik

- Şifreler hash'lenmiş olarak saklanır (ASP.NET Core Identity)
- Rol tabanlı yetkilendirme
- Anti-forgery token koruması
- HTTPS zorunluluğu

## Geliştirme

Bu proje .NET 8 MVC kullanılarak geliştirilmiştir. Tema özelleştirmeleri için `wwwroot/css/site.css` dosyasını düzenleyebilirsiniz.

## Lisans

Bu proje eğitim amaçlı geliştirilmiştir.
# ForumApplication
