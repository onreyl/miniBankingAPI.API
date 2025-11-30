using MediatR;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Enums;
using miniBankingAPI.Domain.Interfaces;

namespace miniBankingAPI.Application.Features.Accounts.Commands.TransferMoney
{
    public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransferMoneyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
        {
            var fromAccount = await _unitOfWork.AccountsRead.GetByIdAsync(request.FromAccountId);
            var toAccount = await _unitOfWork.AccountsRead.GetByIdAsync(request.ToAccountId);

            if (fromAccount == null || toAccount == null)
                throw new Exception("Account not found");

            if (!fromAccount.CanTransferTo(toAccount))
                throw new Exception("Cannot transfer between different currencies or inactive accounts");

            fromAccount.Withdraw(request.Amount);
            toAccount.Deposit(request.Amount);

            _unitOfWork.AccountsWrite.Update(fromAccount);
            _unitOfWork.AccountsWrite.Update(toAccount);

            var transaction = new Transaction
            {
                AccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                TransactionType = TransactionType.Transfer,
                Description = request.Description ?? "Money transfer",
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.TransactionsWrite.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
