using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace miniBankingAPI.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }  // TC No
        public string Email { get; set; }
        public string Phone { get; set; }
        
        // Navigation Properties
        public ICollection<Account> Accounts { get; set; }
    }
}
