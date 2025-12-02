# 🧪 Unit Test Guide - xUnit + Moq

## 🎯 Unit Test Nedir?

**Unit Test**, kodunun en küçük parçalarını (method, class) izole bir şekilde test etmektir.

### Neden Önemli?
- ✅ **Hata Önleme:** Kod değiştiğinde hataları erken yakala
- ✅ **Refactoring:** Güvenle kod değiştir
- ✅ **Dokümantasyon:** Test, kodun nasıl çalıştığını gösterir
- ✅ **Code Quality:** Test edilebilir kod = iyi kod
- ✅ **Mülakat:** Senior pozisyonlarda mutlaka sorarlar

---

## 📊 Test Piramidi

```
        /\
       /  \      E2E Tests (Az)
      /____\
     /      \    Integration Tests (Orta)
    /________\
   /          \  Unit Tests (Çok)
  /____________\
```

**Unit Tests:** En hızlı, en çok yazılmalı  
**Integration Tests:** Orta hızlı, orta sayıda  
**E2E Tests:** En yavaş, en az sayıda

---

## 🔧 Kullanılan Teknolojiler

### 1. **xUnit**
- .NET için en popüler test framework'ü
- Microsoft tarafından önerilen
- Paralel test çalıştırma

### 2. **Moq**
- Mocking library
- Dependency'leri taklit eder
- Interface'leri mock'lar

### 3. **FluentAssertions**
- Okunabilir assertion'lar
- Daha iyi hata mesajları
- `Should()` syntax

---

## 📝 Test Yapısı: AAA Pattern

```csharp
[Fact]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange (Hazırlık)
    var account = new Account { Balance = 100 };

    // Act (İşlem)
    account.Deposit(50);

    // Assert (Doğrulama)
    account.Balance.Should().Be(150);
}
```

### Arrange (Hazırlık):
- Test için gerekli nesneleri oluştur
- Mock'ları ayarla
- Test verilerini hazırla

### Act (İşlem):
- Test edilecek metodu çağır
- Tek bir işlem olmalı

### Assert (Doğrulama):
- Sonucu kontrol et
- Beklenen davranışı doğrula

---

## 🎯 Test İsimlendirme

### Format:
```
MethodName_Scenario_ExpectedResult
```

### Örnekler:
```csharp
Deposit_ShouldIncreaseBalance
Withdraw_WithSufficientBalance_ShouldDecreaseBalance
Withdraw_WithInsufficientBalance_ShouldThrowException
CanTransferTo_WithDifferentCurrency_ShouldReturnFalse
```

---

## 💻 Kod Örnekleri

### 1. Basit Unit Test (Domain Entity)

```csharp
public class AccountTests
{
    [Fact]
    public void Deposit_ShouldIncreaseBalance()
    {
        // Arrange
        var account = new Account
        {
            Balance = 100,
            IsActive = true
        };

        // Act
        account.Deposit(50);

        // Assert
        account.Balance.Should().Be(150);
    }
}
```

**Açıklama:**
- `[Fact]`: Bu bir test metodu
- `Should().Be()`: FluentAssertions syntax
- Dependency yok, direkt entity test ediliyor

---

### 2. Exception Test

```csharp
[Fact]
public void Withdraw_WithInsufficientBalance_ShouldThrowException()
{
    // Arrange
    var account = new Account { Balance = 50 };

    // Act
    var act = () => account.Withdraw(100);

    // Assert
    act.Should().Throw<Exception>()
        .WithMessage("Insufficient balance");
}
```

**Açıklama:**
- `() => account.Withdraw(100)`: Lambda expression (exception yakalamak için)
- `Should().Throw<Exception>()`: Exception bekliyoruz
- `WithMessage()`: Hata mesajını kontrol et

---

### 3. Mock Kullanımı (Handler Test)

```csharp
public class TransferMoneyCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAccountReadRepository> _accountReadRepoMock;
    private readonly TransferMoneyCommandHandler _handler;

    public TransferMoneyCommandHandlerTests()
    {
        // Mock'ları oluştur
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _accountReadRepoMock = new Mock<IAccountReadRepository>();
        
        // Mock'ları bağla
        _unitOfWorkMock.Setup(u => u.AccountsRead)
            .Returns(_accountReadRepoMock.Object);
        
        // Handler'ı oluştur (mock'larla)
        _handler = new TransferMoneyCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidTransfer_ShouldReturnTrue()
    {
        // Arrange
        var fromAccount = new Account { Id = 1, Balance = 1000 };
        var toAccount = new Account { Id = 2, Balance = 500 };
        
        // Mock davranışını ayarla
        _accountReadRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(fromAccount);
        _accountReadRepoMock.Setup(r => r.GetByIdAsync(2))
            .ReturnsAsync(toAccount);

        var command = new TransferMoneyCommand
        {
            FromAccountId = 1,
            ToAccountId = 2,
            Amount = 100
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        fromAccount.Balance.Should().Be(900);
        toAccount.Balance.Should().Be(600);
        
        // Mock'un çağrıldığını doğrula
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
```

**Açıklama:**
- `Mock<IUnitOfWork>`: IUnitOfWork'ü taklit et
- `Setup()`: Mock'un nasıl davranacağını ayarla
- `ReturnsAsync()`: Async metod için dönüş değeri
- `Verify()`: Metodun çağrıldığını doğrula
- `Times.Once`: Tam 1 kez çağrılmalı

---

## 🔍 Mock Setup Örnekleri

### 1. Basit Return
```csharp
_mockRepo.Setup(r => r.GetByIdAsync(1))
    .ReturnsAsync(account);
```

### 2. Any Parameter
```csharp
_mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
    .ReturnsAsync((Account?)null);
```

### 3. Conditional Return
```csharp
_mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(id => id > 0)))
    .ReturnsAsync(account);
```

### 4. Throw Exception
```csharp
_mockRepo.Setup(r => r.GetByIdAsync(999))
    .ThrowsAsync(new Exception("Not found"));
```

---

## ✅ FluentAssertions Örnekleri

### Equality
```csharp
result.Should().Be(100);
result.Should().NotBe(0);
```

### Boolean
```csharp
result.Should().BeTrue();
result.Should().BeFalse();
```

### Null
```csharp
result.Should().BeNull();
result.Should().NotBeNull();
```

### Collections
```csharp
list.Should().HaveCount(3);
list.Should().Contain(item);
list.Should().BeEmpty();
```

### Exceptions
```csharp
act.Should().Throw<Exception>();
act.Should().Throw<Exception>().WithMessage("Error");
act.Should().NotThrow();
```

### Async
```csharp
await act.Should().ThrowAsync<Exception>();
```

---

## 🧪 Test Çalıştırma

### Tüm Testleri Çalıştır:
```bash
dotnet test
```

### Verbose Output:
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Sadece Bir Test:
```bash
dotnet test --filter "FullyQualifiedName~AccountTests"
```

### Code Coverage:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📊 Test Sonuçları

### Başarılı Test:
```
Passed!  - Failed:     0, Passed:    10, Skipped:     0, Total:    10
```

### Başarısız Test:
```
Failed!  - Failed:     2, Passed:     8, Skipped:     0, Total:    10

Error Message:
   Expected a <System.InvalidOperationException> to be thrown, but found <System.Exception>
```

---

## 🎯 Mülakatta Sorulabilecekler

### Soru 1: "Unit test nedir, neden yazarsın?"

**Cevap:**
> "Unit test, kodun en küçük parçalarını izole bir şekilde test etmektir. Yazma sebepleri: Hataları erken yakalamak, refactoring yaparken güven sağlamak, kodun dokümantasyonu olması. Ben xUnit + Moq kullanıyorum."

### Soru 2: "Mock nedir, ne zaman kullanırsın?"

**Cevap:**
> "Mock, dependency'leri taklit etmektir. Örneğin Handler test ederken, gerçek database'e bağlanmak yerine IUnitOfWork'ü mock'larım. Bu sayede test hızlı ve izole olur. Moq library kullanıyorum."

### Soru 3: "AAA pattern nedir?"

**Cevap:**
> "Arrange-Act-Assert. Test yapısı: Arrange'de test verilerini hazırlarım, Act'te test edilecek metodu çağırırım, Assert'te sonucu doğrularım. Her test bu yapıda olmalı."

### Soru 4: "Test coverage ne olmalı?"

**Cevap:**
> "İdeal %80-90 arası. Ama %100 coverage her zaman iyi değil. Kritik business logic'i test etmek önemli. Domain entities, handlers, business rules mutlaka test edilmeli."

### Soru 5: "Integration test ile unit test farkı nedir?"

**Cevap:**
> "Unit test tek bir birimi test eder, dependency'ler mock'lanır. Integration test birden fazla komponenti birlikte test eder, gerçek database kullanılır. Unit test hızlı, integration test yavaş."

---

## 📁 Proje Yapısı

```
miniBankingAPI.Tests/
├── Domain/
│   └── AccountTests.cs          # Entity testleri
├── Application/
│   └── TransferMoneyCommandHandlerTests.cs  # Handler testleri
└── miniBankingAPI.Tests.csproj
```

---

## 🚀 Yazılan Testler

### Domain Tests (AccountTests.cs):
1. ✅ `Deposit_ShouldIncreaseBalance`
2. ✅ `Withdraw_WithSufficientBalance_ShouldDecreaseBalance`
3. ✅ `Withdraw_WithInsufficientBalance_ShouldThrowException`
4. ✅ `CanTransferTo_WithSameCurrencyAndActiveAccounts_ShouldReturnTrue`
5. ✅ `CanTransferTo_WithDifferentCurrency_ShouldReturnFalse`
6. ✅ `CanTransferTo_WithInactiveAccount_ShouldReturnFalse`

### Application Tests (TransferMoneyCommandHandlerTests.cs):
1. ✅ `Handle_WithValidTransfer_ShouldReturnTrue`
2. ✅ `Handle_WithAccountNotFound_ShouldThrowException`
3. ✅ `Handle_WithDifferentCurrency_ShouldThrowException`
4. ✅ `Handle_WithInsufficientBalance_ShouldThrowException`

**Toplam: 10 Test - Hepsi Başarılı! ✅**

---

## 📝 Best Practices

### ✅ DOĞRU:

```csharp
// 1. Anlamlı test isimleri
[Fact]
public void Withdraw_WithInsufficientBalance_ShouldThrowException()

// 2. AAA pattern kullan
// Arrange
var account = new Account { Balance = 100 };
// Act
account.Deposit(50);
// Assert
account.Balance.Should().Be(150);

// 3. Tek bir şey test et
[Fact]
public void Deposit_ShouldIncreaseBalance() // Sadece deposit test ediliyor

// 4. FluentAssertions kullan
result.Should().Be(100); // Okunabilir
```

### ❌ YANLIŞ:

```csharp
// 1. Belirsiz test isimleri
[Fact]
public void Test1() // ❌ Ne test ediliyor?

// 2. Birden fazla şey test et
[Fact]
public void AccountTest() // ❌ Deposit + Withdraw + Transfer hepsi

// 3. Assert kullanma
Assert.Equal(100, result); // ❌ Daha az okunabilir

// 4. Test'te business logic
[Fact]
public void Test()
{
    if (account.Balance > 100) // ❌ Test'te if kullanma
        account.Deposit(50);
}
```

---

## 🎓 Özet

1. **xUnit** → Test framework
2. **Moq** → Mocking library
3. **FluentAssertions** → Okunabilir assertion'lar
4. **AAA Pattern** → Arrange-Act-Assert
5. **Mock** → Dependency'leri taklit et
6. **Verify** → Mock'un çağrıldığını doğrula
7. **Test İsimlendirme** → MethodName_Scenario_ExpectedResult

---

## 📚 Daha Fazla Bilgi

- [xUnit Dokümantasyonu](https://xunit.net/)
- [Moq Dokümantasyonu](https://github.com/moq/moq4)
- [FluentAssertions Dokümantasyonu](https://fluentassertions.com/)
- [Unit Testing Best Practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

---

## 🎉 Sonuç

**10 Unit Test Yazıldı - Hepsi Başarılı!** ✅

Artık projen **production-ready** ve **test coverage** var!

Mülakatta rahatlıkla "Unit test yazdım, xUnit + Moq kullandım" diyebilirsin! 🚀
