using MediatR;
using Microsoft.Extensions.Logging;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Enums;
using miniBankingAPI.Domain.Interfaces;

namespace miniBankingAPI.Application.Features.Accounts.Commands.TransferMoney
{
    public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransferMoneyCommandHandler> _logger;

        public TransferMoneyCommandHandler(IUnitOfWork unitOfWork, ILogger<TransferMoneyCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Transfer initiated: {Amount} from Account {FromAccountId} to Account {ToAccountId}", 
                request.Amount, request.FromAccountId, request.ToAccountId);

            var fromAccount = await _unitOfWork.AccountsRead.GetByIdAsync(request.FromAccountId);
            var toAccount = await _unitOfWork.AccountsRead.GetByIdAsync(request.ToAccountId);

            if (fromAccount == null || toAccount == null)
            {
                _logger.LogError("Transfer failed - Account not found. FromAccountId: {FromAccountId}, ToAccountId: {ToAccountId}", 
                    request.FromAccountId, request.ToAccountId);
                throw new Exception("Account not found");
            }

            if (!fromAccount.CanTransferTo(toAccount))
            {
                _logger.LogWarning("Transfer failed - Invalid transfer conditions. FromAccountId: {FromAccountId}, ToAccountId: {ToAccountId}", 
                    request.FromAccountId, request.ToAccountId);
                throw new Exception("Cannot transfer between different currencies or inactive accounts");
            }

            fromAccount.Withdraw(request.Amount);
            toAccount.Deposit(request.Amount);

            _unitOfWork.AccountsWrite.Update(fromAccount);
            _unitOfWork.AccountsWrite.Update(toAccount);

            var newTransaction = new Transaction
            {
                AccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                TransactionType = TransactionType.Transfer,
                Description = request.Description ?? "Money transfer",
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.TransactionsWrite.AddAsync(newTransaction);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Transfer completed successfully: {Amount} from Account {FromAccountId} to Account {ToAccountId}", 
                request.Amount, request.FromAccountId, request.ToAccountId);

            return true;
        }
    }
}
