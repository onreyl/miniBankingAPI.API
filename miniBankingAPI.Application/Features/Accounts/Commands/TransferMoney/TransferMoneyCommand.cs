using MediatR;

namespace miniBankingAPI.Application.Features.Accounts.Commands.TransferMoney
{
    public class TransferMoneyCommand : IRequest<bool>
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
