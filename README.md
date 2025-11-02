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

## Varsayılan Admin Hesabı

Uygulama ilk çalıştırıldığında otomatik olarak bir admin hesabı oluşturulur:

- **E-posta:** admin@forumapp.com
- **Şifre:** Admin123!


## Geliştirme

Bu proje .NET 8 MVC kullanılarak geliştirilmiştir. Tema özelleştirmeleri için `wwwroot/css/site.css` dosyasını düzenleyebilirsiniz.

## Lisans

Bu proje eğitim amaçlı geliştirilmiştir.
# ForumApplication
