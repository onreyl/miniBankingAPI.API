using MediatR;
using miniBankingAPI.Application.DTOs;

namespace miniBankingAPI.Application.Features.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQuery : IRequest<AccountDto>
    {
        public int AccountId { get; set; }
    }
}
