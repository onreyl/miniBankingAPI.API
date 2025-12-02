# 📝 Logging Guide - Serilog

## 🎯 Logging Nedir?

**Logging**, uygulamanın çalışma zamanında olan olayları kaydetme işlemidir.

### Neden Kullanılır?
- ✅ **Hata Takibi:** Production'da hata olduğunda ne olduğunu görebilirsin
- ✅ **Performans:** Hangi işlemler yavaş çalışıyor?
- ✅ **Güvenlik:** Kim ne zaman login oldu?
- ✅ **Audit:** Para transferi gibi kritik işlemleri kaydet

---

## 📊 Log Seviyeleri

```csharp
Trace       → En detaylı (her şey) - Sadece development
Debug       → Geliştirme sırasında debug için
Information → Normal bilgi (Login oldu, hesap oluşturuldu)
Warning     → Uyarı (Bakiye düşük, yavaş işlem)
Error       → Hata (Exception, işlem başarısız)
Critical    → Kritik hata (Database bağlantısı koptu, sistem çöktü)
```

### Hangi Seviyeyi Ne Zaman Kullanmalı?

| Seviye | Ne Zaman | Örnek |
|--------|----------|-------|
| **Information** | Normal işlemler | "User ahmet logged in", "Account created" |
| **Warning** | Potansiyel sorun | "Failed login attempt", "Low balance" |
| **Error** | Hata oluştu | "Account not found", "Invalid transfer" |
| **Critical** | Sistem çöktü | "Database connection failed", "Out of memory" |

---

## 🔧 Serilog Yapılandırması

### appsettings.json

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",  // Varsayılan seviye
      "Override": {
        "Microsoft": "Warning",  // Microsoft loglarını azalt
        "System": "Warning"      // System loglarını azalt
      }
    },
    "WriteTo": [
      {
        "Name": "Console",  // Console'a yaz
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",  // Dosyaya yaz
        "Args": {
          "path": "Logs/log-.txt",  // Logs klasörüne
          "rollingInterval": "Day"   // Her gün yeni dosya
        }
      }
    ]
  }
}
```

### Açıklamalar:

**Console Sink:**
- Geliştirme sırasında logları görmek için
- `outputTemplate`: Log formatı

**File Sink:**
- Production'da logları saklamak için
- `path`: "Logs/log-.txt" → `Logs/log-20231201.txt` şeklinde oluşur
- `rollingInterval: Day`: Her gün yeni dosya oluşturur

---

## 💻 Kod Örnekleri

### 1. Constructor'da ILogger Enjekte Et

```csharp
public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;

    public AuthService(ILogger<AuthService> logger)
    {
        _logger = logger;
    }
}
```

### 2. Information Log (Normal İşlem)

```csharp
_logger.LogInformation("User {Username} logged in successfully", username);
```

**Çıktı:**
```
[14:30:25 INF] User ahmet logged in successfully
```

### 3. Warning Log (Uyarı)

```csharp
_logger.LogWarning("Failed login attempt for user: {Username}", username);
```

**Çıktı:**
```
[14:30:25 WRN] Failed login attempt for user: ahmet
```

### 4. Error Log (Hata)

```csharp
try
{
    // İşlem
}
catch (Exception ex)
{
    _logger.LogError(ex, "Transfer failed for Account {AccountId}", accountId);
    throw;
}
```

**Çıktı:**
```
[14:30:25 ERR] Transfer failed for Account 123
System.Exception: Insufficient balance
   at TransferMoneyCommandHandler.Handle()
```

### 5. Structured Logging (Önemli!)

```csharp
// ❌ YANLIŞ:
_logger.LogInformation("Transfer: " + amount + " from " + fromId + " to " + toId);

// ✅ DOĞRU:
_logger.LogInformation("Transfer: {Amount} from Account {FromAccountId} to Account {ToAccountId}", 
    amount, fromId, toId);
```

**Neden Doğru?**
- JSON formatında saklanır
- Filtreleme yapabilirsin
- Arama yapabilirsin

---

## 📁 Log Dosyaları

### Nerede Saklanır?

```
miniBankingAPI.API/
├── Logs/
│   ├── log-20231201.txt  ← Bugünün logu
│   ├── log-20231130.txt  ← Dünün logu
│   └── log-20231129.txt  ← Önceki gün
```

### Log Dosyası İçeriği:

```
2023-12-01 14:30:25.123 +03:00 [INF] Starting Mini Banking API...
2023-12-01 14:30:26.456 +03:00 [INF] Application built successfully
2023-12-01 14:30:27.789 +03:00 [INF] Application started successfully
2023-12-01 14:31:15.234 +03:00 [INF] Login attempt for user: ahmet
2023-12-01 14:31:15.567 +03:00 [INF] User ahmet logged in successfully
2023-12-01 14:32:45.890 +03:00 [INF] Transfer initiated: 100 from Account 1 to Account 2
2023-12-01 14:32:46.123 +03:00 [INF] Transfer completed successfully: 100 from Account 1 to Account 2
```

---

## 🎯 Mülakatta Sorulabilecekler

### Soru 1: "Logging neden önemlidir?"

**Cevap:**
> "Production'da hata olduğunda ne olduğunu görmek için. Örneğin kullanıcı 'para transferi çalışmıyor' dediğinde, loglardan hangi hesaptan hangi hesaba ne kadar para transfer etmeye çalıştığını, hangi hatayı aldığını görebilirim."

### Soru 2: "Hangi log seviyesini ne zaman kullanırsın?"

**Cevap:**
> "Information: Normal işlemler (login, hesap oluşturma)
> Warning: Potansiyel sorunlar (başarısız login denemesi)
> Error: Hatalar (hesap bulunamadı, yetersiz bakiye)
> Critical: Sistem çökmesi (database bağlantısı koptu)"

### Soru 3: "Structured logging nedir?"

**Cevap:**
> "String concatenation yerine placeholder kullanmak. Örneğin 'User ' + username yerine 'User {Username}' yazmak. Bu sayede loglar JSON formatında saklanır ve filtreleme/arama yapılabilir."

### Soru 4: "Production'da logları nasıl yönetirsin?"

**Cevap:**
> "Serilog ile dosyaya yazarım, her gün yeni dosya oluşur. Daha büyük projelerde Elasticsearch veya Application Insights gibi merkezi log sistemleri kullanırım."

---

## 🚀 Kullanım Örnekleri

### AuthService

```csharp
public async Task<string> Login(string username, string password)
{
    _logger.LogInformation("Login attempt for user: {Username}", username);
    
    var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
    
    if (user == null || !VerifyPassword(password, user.PasswordHash))
    {
        _logger.LogWarning("Failed login attempt for user: {Username}", username);
        throw new UnauthorizedAccessException("Invalid username or password");
    }

    _logger.LogInformation("User {Username} logged in successfully", username);
    return GenerateJwtToken(user.Id, user.Username);
}
```

### TransferMoneyCommandHandler

```csharp
public async Task<bool> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
{
    _logger.LogInformation("Transfer initiated: {Amount} from Account {FromAccountId} to Account {ToAccountId}", 
        request.Amount, request.FromAccountId, request.ToAccountId);

    // İşlemler...

    _logger.LogInformation("Transfer completed successfully: {Amount} from Account {FromAccountId} to Account {ToAccountId}", 
        request.Amount, request.FromAccountId, request.ToAccountId);

    return true;
}
```

---

## 📝 Best Practices

### ✅ DOĞRU:

```csharp
// 1. Structured logging kullan
_logger.LogInformation("User {Username} created account {AccountId}", username, accountId);

// 2. Anlamlı mesajlar yaz
_logger.LogError("Transfer failed - Insufficient balance. AccountId: {AccountId}, Required: {Amount}", accountId, amount);

// 3. Exception'ı logla
_logger.LogError(ex, "Failed to process payment for Order {OrderId}", orderId);
```

### ❌ YANLIŞ:

```csharp
// 1. String concatenation kullanma
_logger.LogInformation("User " + username + " created account " + accountId);

// 2. Belirsiz mesajlar yazma
_logger.LogError("Error");

// 3. Hassas bilgi loglama
_logger.LogInformation("User password: {Password}", password); // ❌ ŞİFRE LOGLAMA!
```

---

## 🔍 Log Analizi

### Başarılı Login:
```
[14:30:25 INF] Login attempt for user: ahmet
[14:30:25 INF] User ahmet logged in successfully
```

### Başarısız Login:
```
[14:30:25 INF] Login attempt for user: ahmet
[14:30:25 WRN] Failed login attempt for user: ahmet
```

### Başarılı Transfer:
```
[14:32:45 INF] Transfer initiated: 100 from Account 1 to Account 2
[14:32:46 INF] Transfer completed successfully: 100 from Account 1 to Account 2
```

### Başarısız Transfer:
```
[14:32:45 INF] Transfer initiated: 100 from Account 1 to Account 2
[14:32:45 ERR] Transfer failed - Account not found. FromAccountId: 1, ToAccountId: 999
```

---

## 🎓 Özet

1. **ILogger<T>** dependency injection ile enjekte edilir
2. **LogInformation** → Normal işlemler
3. **LogWarning** → Uyarılar
4. **LogError** → Hatalar
5. **Structured logging** → `{Placeholder}` kullan
6. **Hassas bilgi loglama** → Asla şifre, kredi kartı loglama!
7. **Log dosyaları** → `Logs/` klasöründe, her gün yeni dosya

---

## 📚 Daha Fazla Bilgi

- [Serilog Dokümantasyonu](https://serilog.net/)
- [ASP.NET Core Logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/)
