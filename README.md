# GeoMap

GeoMap, harita üzerinde çizim yapmanı ve bu çizimleri veritabanına GeoJSON formatında kaydetmeni sağlayan .NET 8 ve Leaflet tabanlı açık kaynaklı bir coğrafi bilgi sistemidir. Uygulama, PostgreSQL + PostGIS kullanarak mekansal verileri yönetir.

## 🔧 Kullanılan Teknolojiler
- ASP.NET Core 8
- Entity Framework Core
- NetTopologySuite
- PostgreSQL + PostGIS
- Leaflet.js
- Bootstrap

## 🚀 Özellikler
- Harita üzerinde nokta, çizgi ve poligon çizimi
- GeoJSON veri formatı ile veri gönderimi
- PostgreSQL + PostGIS üzerinde geometri saklama
- Çizilen geometrilerin listelenmesi, düzenlenmesi ve silinmesi

## 📦 Kurulum Adımları

### 1. Depoyu Klonlayın
```bash
git clone https://github.com/mkadirgulgun/geomap.git
cd geomap
```

### 2. Bağımlılıkları Yükleyin
```bash
dotnet restore
```

### 3. Veritabanı Ayarlarını Yapın
`appsettings.json` dosyasındaki bağlantı bilgilerini kendi PostgreSQL ayarlarınıza göre düzenleyin:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=geomapdb;Username=postgres;Password=yourpassword"
}
```
> ⚠️ Veritabanınızda PostGIS eklentisinin yüklü olması gerekir.

### 4. Veritabanını Oluşturun
```bash
dotnet ef database update
```

### 5. Uygulamayı Çalıştırın
```bash
dotnet run
```

## 🔐 Giriş Bilgileri
Bu uygulama şu anda kimlik doğrulama içermemektedir. Sayfaya doğrudan erişim sağlanabilir.
