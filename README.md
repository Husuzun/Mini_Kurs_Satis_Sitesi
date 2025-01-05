# Mini Kurs Satış Sitesi

Bu proje, çevrimiçi kurs satış platformu için geliştirilmiş bir web uygulamasıdır. Backend .NET Core ve Frontend React kullanılarak geliştirilmiştir.

## Teknolojiler

### Backend
- .NET Core 9.0
- Entity Framework Core
- SQL Server
- JWT Authentication
- AutoMapper
- FluentValidation

### Frontend
- React 19.0.0
- Material-UI 6.3.0
- React Router DOM 7.1.1
- JWT Decode

## Kurulum Adımları

### Backend Kurulumu

1. Projeyi klonlayın:
```bash
git clone [repository-url]
cd Mini_Kurs_Satis_Sitesi
```

2. Visual Studio'da `Mini_Kurs_Satis_Sitesi.sln` dosyasını açın.

4. Package Manager Console'da aşağıdaki komutları çalıştırın:
```bash
Update-Database
```

5. Projeyi çalıştırın.

### Frontend Kurulumu

1. Frontend klasörüne gidin:
```bash
cd mini_kurs_satis_sitesi_frontend
```

2. Bağımlılıkları yükleyin:
```bash
npm install
```

4. Uygulamayı başlatın:
```bash
npm start
```

Browser'da otomatik olarak `http://localhost:3000` adresinde açılacaktır.

## Seed Data

Proje başlatıldığında otomatik olarak aşağıdaki test verileri oluşturulacaktır:

### Kullanıcılar
1. **Fatih Çakıroğlu (Eğitmen)**
   - Email: fatih@example.com
   - Şifre: Password12*

2. **Ahmet Kaya (Eğitmen)**
   - Email: ahmet@example.com
   - Şifre: Password12*

3. **Hüseyin Uzun (Kullanıcı)**
   - Email: huseyin@example.com
   - Şifre: Password12*

## API Endpoints

Tüm API endpoint'leri Postman koleksiyonunda mevcuttur. Koleksiyonu içe aktarmak için:

1. Postman'i açın
2. Collections > Import düğmesine tıklayın
3. `Mini_Kurs_Satis_Sitesi.postman_collection.json` dosyasını seçin

## Önemli Notlar

1. Backend varsayılan olarak `https://localhost:7261` adresinde çalışır
2. Frontend varsayılan olarak `http://localhost:3000` adresinde çalışır
3. Veritabanı migration'ları otomatik olarak uygulanacaktır
4. Test kullanıcıları ve kursları otomatik olarak oluşturulacaktır
