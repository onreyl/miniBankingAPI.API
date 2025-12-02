# 🏦 Mini Banking API - Proje Özeti

## 📋 Genel Bakış

**Proje Adı:** Mini Banking API  
**Teknoloji:** .NET 10, C#  
**Mimari:** Clean Architecture + CQRS  
**Seviye:** Mid-Senior Level  
**Durum:** ✅ Production Ready

---

## 🎯 Özellikler

### ✅ Tamamlanan Özellikler:

1. **Clean Architecture**
   - Domain, Application, Infrastructure, API katmanları
   - Dependency Inversion Principle
   - Separation of Concerns

2. **CQRS + MediatR**
   - Commands (CreateAccount, TransferMoney)
   - Queries (GetAccountBalance)
   - Handler pattern

3. **Repository Pattern + Unit of Work**
   - IAccountReadRepository, IAccountWriteRepository
   - ICustomerReadRepository, ICustomerWriteRepository
   - ITransactionReadRepository, ITransactionWriteRepository
   - IUnitOfWork (Transaction management)

4. **JWT Authentication**
   - Login/Register endpoints
   - Token generation
   - BCrypt password hashing (workFactor: 12)
   - Bearer token authentication

5. **FluentValidation**
   - CreateAccountCommandValidator
   - TransferMoneyCommandValidator
   - Automatic validation

6. **Global Exception Handler**
   - Centralized error handling
   - Structured error responses
   - HTTP status code mapping

7. **Logging (Serilog)**
   - Console + File logging
   - Structured logging
   - Daily rolling log files
   - Log levels: Information, Warning, Error, Critical

8. **CORS**
   - AllowAll policy
   - Frontend integration ready

9. **Transaction Management**
   - Database transactions
   - Rollback on error
   - Concurrency control (RowVersion)

10. **Entity Framework Core**
    - Code-First approach
    - Migrations
    - SQL Server

---

## 🏗️ Proje Yapısı

```
miniBankingAPI.API/
├── miniBankingAPI.API/              # Presentation Layer
│   ├── Controllers/
│   │   ├── AuthController.cs        # Login, Register
│   │   └── AccountController.cs     # Account operations
│   ├── Middlewares/
│   │   └── GlobalExceptionHandler.cs
│   └── Program.cs
│
├── miniBankingAPI.Application/      # Application Layer
│   ├── Features/
│   │   └── Accounts/
│   │       ├── Commands/
│   │       │   ├── CreateAccount/
│   │       │   └── TransferMoney/
│   │       └── Queries/
│   │           └── GetAccountBalance/
│   └── DTOs/
│
├── miniBankingAPI.Domain/           # Domain Layer
│   ├── Entities/
│   │   ├── Account.cs
│   │   ├── Customer.cs
│   │   ├── Transaction.cs
│   │   └── User.cs
│   ├── Enums/
│   │   ├── CurrencyType.cs
│   │   └── TransactionType.cs
│   └── Interfaces/
│       ├── IRepositories/
│       ├── IAuthService.cs
│       └── IUnitOfWork.cs
│
└── miniBankingAPI.Infrastructure/   # Infrastructure Layer
    ├── Persistence/
    │   ├── Data/
    │   │   └── BankingDbContext.cs
    │   ├── Configurations/
    │   └── Repositories/
    │       └── UnitOfWork.cs
    └── Services/
        └── AuthService.cs
```

---

## 🔧 Teknolojiler

### Backend:
- **.NET 10**
- **C# 12**
- **ASP.NET Core Web API**

### Database:
- **Entity Framework Core 10**
- **SQL Server**
- **Code-First Migrations**

### Patterns & Principles:
- **Clean Architecture**
- **CQRS (Command Query Responsibility Segregation)**
- **Repository Pattern**
- **Unit of Work Pattern**
- **Mediator Pattern**
- **Dependency Injection**
- **SOLID Principles**

### Libraries:
- **MediatR** - CQRS implementation
- **FluentValidation** - Input validation
- **Serilog** - Logging
- **BCrypt.Net** - Password hashing
- **JWT Bearer** - Authentication
- **Swashbuckle** - Swagger/OpenAPI

---

## 📊 API Endpoints

### Authentication:
```http
POST /api/auth/login
POST /api/auth/register
```

### Accounts:
```http
GET  /api/account/{id}           # Get account balance
POST /api/account                # Create account
POST /api/account/transfer       # Transfer money
```

---

## 🔐 Güvenlik

1. **JWT Authentication**
   - Token-based authentication
   - Secure token generation
   - Token expiration (60 minutes)

2. **Password Security**
   - BCrypt hashing
   - WorkFactor: 12 (4096 iterations)
   - Salt included

3. **Input Validation**
   - FluentValidation
   - Request validation
   - Business rule validation

4. **Exception Handling**
   - Global exception handler
   - No sensitive data in errors
   - Structured error responses

---

## 📝 Logging

### Log Locations:
- **Console:** Development debugging
- **File:** `Logs/log-YYYYMMDD.txt`

### Log Levels:
- **Information:** Normal operations (login, account creation)
- **Warning:** Potential issues (failed login attempts)
- **Error:** Errors (account not found, invalid transfer)
- **Critical:** System failures (database connection lost)

### Example Logs:
```
[14:30:25 INF] Starting Mini Banking API...
[14:31:15 INF] Login attempt for user: ahmet
[14:31:15 INF] User ahmet logged in successfully
[14:32:45 INF] Transfer initiated: 100 from Account 1 to Account 2
[14:32:46 INF] Transfer completed successfully
```

---

## 🚀 Nasıl Çalıştırılır?

### 1. Database Migration:
```bash
cd miniBankingAPI.Infrastructure
dotnet ef database update
```

### 2. Uygulamayı Çalıştır:
```bash
cd miniBankingAPI.API
dotnet run
```

### 3. Swagger'a Git:
```
https://localhost:5001
```

### 4. Postman ile Test Et:
```http
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "username": "ahmet",
  "password": "123456"
}
```

---

## 💡 Mülakat İçin Hazırlık

### Teknik Sorular:

**Q: Neden Clean Architecture kullandın?**
> "Katmanlar arası bağımlılığı azaltmak için. Domain katmanı hiçbir şeye bağımlı değil. Infrastructure ve Application katmanları Domain'e bağımlı. Bu sayede test edilebilir ve değiştirilebilir bir yapı oluştu."

**Q: CQRS nedir, neden kullandın?**
> "Command Query Responsibility Segregation. Okuma ve yazma işlemlerini ayırdım. CreateAccount, TransferMoney command'lar, GetAccountBalance query. Bu sayede her işlem kendi sorumluluğuna odaklanıyor."

**Q: Unit of Work pattern'i nasıl kullandın?**
> "Tüm repository'leri tek bir UnitOfWork üzerinden yönetiyorum. SaveChangesAsync() ile tüm değişiklikler tek transaction'da kaydediliyor. Bu sayede atomicity sağlanıyor."

**Q: JWT nasıl çalışıyor?**
> "Kullanıcı login olduğunda, userId ve username'i içeren bir token oluşturuyorum. Token 3 bölümden oluşuyor: Header (algoritma), Payload (claims), Signature (güvenlik). SecretKey ile imzalanıyor, değiştirilemez."

**Q: Logging neden önemli?**
> "Production'da hata olduğunda ne olduğunu görmek için. Serilog ile structured logging yapıyorum. Loglar hem console'a hem dosyaya yazılıyor. Login, transfer gibi kritik işlemleri logluyorum."

**Q: Concurrency sorunlarını nasıl çözüyorsun?**
> "RowVersion kullanıyorum. Entity'de byte[] RowVersion property'si var. Aynı anda iki kullanıcı aynı hesabı güncellemeye çalışırsa, ikincisi DbUpdateConcurrencyException alır."

---

## 📈 Performans

### Optimizasyonlar:
- ✅ Async/await kullanımı
- ✅ Connection pooling (EF Core)
- ✅ Structured logging (performanslı)
- ✅ Repository pattern (caching eklenebilir)

### İyileştirilebilir:
- ⚠️ Redis cache eklenebilir
- ⚠️ Response caching eklenebilir
- ⚠️ Database indexing optimize edilebilir

---

## 🧪 Test

### Mevcut:
- ❌ Unit tests yok

### Eklenebilir:
- xUnit + Moq ile unit tests
- Integration tests
- E2E tests

---

## 📚 Dokümantasyon

- ✅ README.md
- ✅ LOGGING_GUIDE.md
- ✅ PROJECT_SUMMARY.md (bu dosya)
- ✅ Swagger/OpenAPI

---

## 🎯 Proje Değerlendirmesi

| Kategori | Puan | Açıklama |
|----------|------|----------|
| **Architecture** | 10/10 | Clean Architecture + CQRS |
| **Design Patterns** | 10/10 | Repository + UnitOfWork + MediatR |
| **Security** | 9/10 | JWT + BCrypt + Validation |
| **Code Quality** | 9/10 | SOLID + Exception Handling |
| **Observability** | 8/10 | Logging (Monitoring yok) |
| **Testing** | 0/10 | Unit test yok |
| **Documentation** | 9/10 | Kapsamlı dokümantasyon |

**TOPLAM: 55/70 = %79** → **İyi Bir Mid-Senior Proje**

---

## 🏆 Güçlü Yönler

1. ✅ **Temiz Mimari:** Clean Architecture + CQRS
2. ✅ **Design Patterns:** Repository, UnitOfWork, Mediator
3. ✅ **Güvenlik:** JWT + BCrypt + Validation
4. ✅ **Logging:** Serilog ile structured logging
5. ✅ **Exception Handling:** Global exception handler
6. ✅ **Transaction Management:** Database transactions + Concurrency control
7. ✅ **CORS:** Frontend entegrasyonu hazır
8. ✅ **Dokümantasyon:** Kapsamlı ve anlaşılır

---

## ⚠️ İyileştirilebilir Yönler

1. ❌ **Unit Tests:** xUnit + Moq ile test coverage
2. ⚠️ **Caching:** Redis cache eklenebilir
3. ⚠️ **Rate Limiting:** DDoS koruması eklenebilir
4. ⚠️ **Health Checks:** Monitoring için endpoint
5. ⚠️ **API Versioning:** Backward compatibility

---

## 🎓 Öğrenilen Konular

1. **Clean Architecture** - Katmanlı mimari
2. **CQRS** - Command/Query separation
3. **Repository Pattern** - Data access abstraction
4. **Unit of Work** - Transaction management
5. **MediatR** - Mediator pattern
6. **JWT** - Token-based authentication
7. **BCrypt** - Password hashing
8. **Serilog** - Structured logging
9. **FluentValidation** - Input validation
10. **Entity Framework Core** - ORM

---

## 📞 İletişim

**Proje:** Mini Banking API  
**Geliştirici:** [Adın]  
**Tarih:** Aralık 2024  
**Durum:** Production Ready ✅

---

## 🚀 Sonuç

Bu proje, **Vakıfbank 2 yıllık .NET Developer pozisyonu** için yeterli seviyededir.

**Güçlü Yönler:**
- Modern mimari (Clean Architecture + CQRS)
- Best practices (Repository, UnitOfWork, Logging)
- Güvenlik (JWT, BCrypt, Validation)
- Dokümantasyon

**Mülakatta Vurgulanacaklar:**
- "Clean Architecture kullandım, katmanlar arası bağımlılık yok"
- "CQRS ile okuma/yazma işlemlerini ayırdım"
- "Unit of Work ile transaction management yaptım"
- "Serilog ile structured logging ekledim"
- "JWT ile secure authentication sağladım"

**Başarılar!** 🎉
