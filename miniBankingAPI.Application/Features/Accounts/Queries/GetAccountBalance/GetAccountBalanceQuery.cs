using MediatR;

namespace miniBankingAPI.Application.Features.Accounts.Queries.GetAccountBalance
{
    public class GetAccountBalanceQuery : IRequest<decimal>
    {
        public int AccountId { get; set; }
    }
}
