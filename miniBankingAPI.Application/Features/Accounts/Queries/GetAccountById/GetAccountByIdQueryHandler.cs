using MediatR;
using miniBankingAPI.Application.DTOs;
using miniBankingAPI.Domain.Interfaces;

namespace miniBankingAPI.Application.Features.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAccountByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.AccountsRead.GetByIdAsync(request.AccountId);
            
            if (account == null)
                throw new Exception("Hesap bulunamadı");

            return new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                CustomerId = account.CustomerId,
                CurrencyType = account.CurrencyType.ToString(),
                Balance = account.Balance,
                IsActive = account.IsActive
            };
        }
    }
}
