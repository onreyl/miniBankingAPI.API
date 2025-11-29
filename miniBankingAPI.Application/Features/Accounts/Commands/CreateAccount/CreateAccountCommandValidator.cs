using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer ID must be valid");

            RuleFor(x => x.CurrencyType)
                .IsInEnum()
                .WithMessage("Select a valid currency");
        }
    }
}
