using FluentValidation;

namespace miniBankingAPI.Application.Features.Accounts.Commands.TransferMoney
{
    public class TransferMoneyCommandValidator : AbstractValidator<TransferMoneyCommand>
    {
        public TransferMoneyCommandValidator()
        {
            RuleFor(x => x.FromAccountId)
                .GreaterThan(0)
                .WithMessage("Gönderen hesap ID geçerli olmalı");

            RuleFor(x => x.ToAccountId)
                .GreaterThan(0)
                .WithMessage("Alıcı hesap ID geçerli olmalı");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Transfer tutarı 0'dan büyük olmalı");

            RuleFor(x => x.FromAccountId)
                .NotEqual(x => x.ToAccountId)
                .WithMessage("Aynı hesaba transfer yapılamaz");
        }
    }
}
