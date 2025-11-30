using miniBankingAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace miniBankingAPI.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string AccountNumber { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public CurrencyType CurrencyType { get; set; }  // TRY, USD, EUR
        public bool IsActive { get; set; }

        // Navigation Properties
        public Customer Customer { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

        // Business Logic Methods
        public void Withdraw(decimal amount)
        {
            if (!IsActive)
                throw new Exception("Account is not active");

            if (amount <= 0)
                throw new Exception("Amount must be greater than zero");

            if (Balance < amount)
                throw new Exception("Insufficient balance");

            Balance -= amount;
        }

        public void Deposit(decimal amount)
        {
            if (!IsActive)
                throw new Exception("Account is not active");

            if (amount <= 0)
                throw new Exception("Amount must be greater than zero");

            Balance += amount;
        }

        public bool CanTransferTo(Account toAccount)
        {
            return IsActive && toAccount.IsActive && CurrencyType == toAccount.CurrencyType;
        }
    }
}

