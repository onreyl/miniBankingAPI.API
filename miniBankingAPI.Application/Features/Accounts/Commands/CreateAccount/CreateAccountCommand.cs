using MediatR;
using miniBankingAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public CurrencyType CurrencyType { get; set; }
    }
}
