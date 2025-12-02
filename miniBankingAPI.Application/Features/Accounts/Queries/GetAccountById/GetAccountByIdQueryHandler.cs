using MediatR;
using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Application.DTOs;
using miniBankingAPI.Infrastructure.Persistence.Data;
using System.Threading;
using System.Threading.Tasks;

namespace miniBankingAPI.Application.Features.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
    {
        private readonly BankingDbContext _context;

        public GetAccountByIdQueryHandler(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken);
            
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
