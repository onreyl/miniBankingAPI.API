using FluentAssertions;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Enums;
using Xunit;

namespace miniBankingAPI.Tests.Domain
{
    public class AccountTests
    {
        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            // Arrange
            var account = new Account
            {
                Id = 1,
                AccountNumber = "TR123456789012",
                Balance = 100,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            // Act
            account.Deposit(50);

            // Assert
            account.Balance.Should().Be(150);
        }

        [Fact]
        public void Withdraw_WithSufficientBalance_ShouldDecreaseBalance()
        {
            // Arrange
            var account = new Account
            {
                Id = 1,
                AccountNumber = "TR123456789012",
                Balance = 100,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            // Act
            account.Withdraw(30);

            // Assert
            account.Balance.Should().Be(70);
        }

        [Fact]
        public void Withdraw_WithInsufficientBalance_ShouldThrowException()
        {
            // Arrange
            var account = new Account
            {
                Id = 1,
                AccountNumber = "TR123456789012",
                Balance = 50,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            // Act & Assert
            var act = () => account.Withdraw(100);
            act.Should().Throw<Exception>()
                .WithMessage("Insufficient balance");
        }

        [Fact]
        public void CanTransferTo_WithSameCurrencyAndActiveAccounts_ShouldReturnTrue()
        {
            // Arrange
            var fromAccount = new Account
            {
                Id = 1,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            var toAccount = new Account
            {
                Id = 2,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            // Act
            var result = fromAccount.CanTransferTo(toAccount);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanTransferTo_WithDifferentCurrency_ShouldReturnFalse()
        {
            // Arrange
            var fromAccount = new Account
            {
                Id = 1,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            var toAccount = new Account
            {
                Id = 2,
                CurrencyType = CurrencyType.USD,
                IsActive = true
            };

            // Act
            var result = fromAccount.CanTransferTo(toAccount);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanTransferTo_WithInactiveAccount_ShouldReturnFalse()
        {
            // Arrange
            var fromAccount = new Account
            {
                Id = 1,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            var toAccount = new Account
            {
                Id = 2,
                CurrencyType = CurrencyType.TRY,
                IsActive = false
            };

            // Act
            var result = fromAccount.CanTransferTo(toAccount);

            // Assert
            result.Should().BeFalse();
        }
    }
}
