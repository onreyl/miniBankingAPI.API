using miniBankingAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public int AccountId { get; set; }
        public TransactionType TransactionType { get; set; }  
        public decimal Amount { get; set; }
        public int? ToAccountId { get; set; } 
        public string Description { get; set; } = string.Empty;

        // Navigation Property
        public Account Account { get; set; } = null!;
    }
}
