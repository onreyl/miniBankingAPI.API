using FluentAssertions;
using Microsoft.Extensions.Logging;
using miniBankingAPI.Application.Features.Accounts.Commands.TransferMoney;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Enums;
using miniBankingAPI.Domain.Interfaces;
using miniBankingAPI.Domain.Interfaces.IRepositories;
using Moq;
using Xunit;

namespace miniBankingAPI.Tests.Application
{
    public class TransferMoneyCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAccountReadRepository> _accountReadRepoMock;
        private readonly Mock<IAccountWriteRepository> _accountWriteRepoMock;
        private readonly Mock<ITransactionWriteRepository> _transactionWriteRepoMock;
        private readonly Mock<ILogger<TransferMoneyCommandHandler>> _loggerMock;
        private readonly TransferMoneyCommandHandler _handler;

        public TransferMoneyCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _accountReadRepoMock = new Mock<IAccountReadRepository>();
            _accountWriteRepoMock = new Mock<IAccountWriteRepository>();
            _transactionWriteRepoMock = new Mock<ITransactionWriteRepository>();
            _loggerMock = new Mock<ILogger<TransferMoneyCommandHandler>>();

            _unitOfWorkMock.Setup(u => u.AccountsRead).Returns(_accountReadRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.AccountsWrite).Returns(_accountWriteRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.TransactionsWrite).Returns(_transactionWriteRepoMock.Object);

            _handler = new TransferMoneyCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidTransfer_ShouldReturnTrue()
        {
            // Arrange
            var fromAccount = new Account
            {
                Id = 1,
                AccountNumber = "TR111111111111",
                Balance = 1000,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            var toAccount = new Account
            {
                Id = 2,
                AccountNumber = "TR222222222222",
                Balance = 500,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            _accountReadRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fromAccount);
            _accountReadRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(toAccount);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var command = new TransferMoneyCommand
            {
                FromAccountId = 1,
                ToAccountId = 2,
                Amount = 100,
                Description = "Test transfer"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            fromAccount.Balance.Should().Be(900);
            toAccount.Balance.Should().Be(600);
            _accountWriteRepoMock.Verify(r => r.Update(fromAccount), Times.Once);
            _accountWriteRepoMock.Verify(r => r.Update(toAccount), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithAccountNotFound_ShouldThrowException()
        {
            // Arrange
            _accountReadRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Account?)null);

            var command = new TransferMoneyCommand
            {
                FromAccountId = 1,
                ToAccountId = 2,
                Amount = 100
            };

            // Act & Assert
            var act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<Exception>().WithMessage("Account not found");
        }

        [Fact]
        public async Task Handle_WithDifferentCurrency_ShouldThrowException()
        {
            // Arrange
            var fromAccount = new Account
            {
                Id = 1,
                CurrencyType = CurrencyType.TRY,
                IsActive = true,
                Balance = 1000
            };

            var toAccount = new Account
            {
                Id = 2,
                CurrencyType = CurrencyType.USD,
                IsActive = true,
                Balance = 500
            };

            _accountReadRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fromAccount);
            _accountReadRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(toAccount);

            var command = new TransferMoneyCommand
            {
                FromAccountId = 1,
                ToAccountId = 2,
                Amount = 100
            };

            // Act & Assert
            var act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Cannot transfer between different currencies or inactive accounts");
        }

        [Fact]
        public async Task Handle_WithInsufficientBalance_ShouldThrowException()
        {
            // Arrange
            var fromAccount = new Account
            {
                Id = 1,
                Balance = 50,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            var toAccount = new Account
            {
                Id = 2,
                Balance = 500,
                CurrencyType = CurrencyType.TRY,
                IsActive = true
            };

            _accountReadRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fromAccount);
            _accountReadRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(toAccount);

            var command = new TransferMoneyCommand
            {
                FromAccountId = 1,
                ToAccountId = 2,
                Amount = 100
            };

            // Act & Assert
            var act = async () => await _handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Insufficient balance");
        }
    }
}
