using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Application.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CurrencyType { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
    }
}
