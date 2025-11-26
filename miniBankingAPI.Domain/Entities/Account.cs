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
    }
}

