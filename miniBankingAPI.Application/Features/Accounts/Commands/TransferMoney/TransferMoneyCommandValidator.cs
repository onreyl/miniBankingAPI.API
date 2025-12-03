using FluentValidation;

namespace miniBankingAPI.Application.Features.Accounts.Commands.TransferMoney
{
    public class TransferMoneyCommandValidator : AbstractValidator<TransferMoneyCommand>
    {
        public TransferMoneyCommandValidator()
        {
            RuleFor(x => x.FromAccountId)
                .GreaterThan(0)
                .WithMessage("From account ID must be valid");

            RuleFor(x => x.ToAccountId)
                .GreaterThan(0)
                .WithMessage("To account ID must be valid");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Transfer amount must be greater than zero");

            RuleFor(x => x.FromAccountId)
                .NotEqual(x => x.ToAccountId)
                .WithMessage("Cannot transfer to the same account");
        }
    }
}
