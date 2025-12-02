using MediatR;
using miniBankingAPI.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace miniBankingAPI.Application.Features.Accounts.Queries.GetAccountBalance
{
    public class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, decimal>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAccountBalanceQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.AccountsRead.GetByIdAsync(request.AccountId);
            
            if (account == null)
                throw new KeyNotFoundException($"Account with ID {request.AccountId} not found");

            return account.Balance;
        }
    }
}
