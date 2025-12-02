using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace miniBankingAPI.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;  // TC No
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        
        // Navigation Properties
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
