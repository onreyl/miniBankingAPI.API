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
                throw new Exception("Hesap bulunamadı");

            if (!fromAccount.IsActive || !toAccount.IsActive)
                throw new Exception("Hesap aktif değil");

            if (fromAccount.Balance < request.Amount)
                throw new Exception("Yetersiz bakiye");

            if (fromAccount.CurrencyType != toAccount.CurrencyType)
                throw new Exception("Farklı para birimleri arasında transfer yapılamaz");

            fromAccount.Balance -= request.Amount;
            toAccount.Balance += request.Amount;

            _unitOfWork.AccountsWrite.Update(fromAccount);
            _unitOfWork.AccountsWrite.Update(toAccount);

            var transaction = new Transaction
            {
                AccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                TransactionType = TransactionType.Transfer,
                Description = request.Description ?? "Para transferi",
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.TransactionsWrite.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
