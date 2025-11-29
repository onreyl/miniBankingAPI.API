using MediatR;
using miniBankingAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
    {
        private IUnitOfWork _unitOfWork;
        public CreateAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var accountNumber = GenerateAccountNumber();

            var account = new Domain.Entities.Account
            {
                CustomerId = request.CustomerId,
                CurrencyType = request.CurrencyType,
                AccountNumber = accountNumber,
                Balance = 0,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.AccountsWrite.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return account.Id;
        }

        private string GenerateAccountNumber()
        {
            return $"TR{DateTime.Now.Ticks % 1000000000000:D12}";
        }

        
    }
}
