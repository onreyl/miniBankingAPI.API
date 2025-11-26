using miniBankingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Domain.Interfaces.IRepositories
{
    public interface ICustomerReadRepository : IReadRepository<Customer>
    {
        Task<Customer> GetByIdentityNumberAsync(string identityNumber);
        Task<Customer> GetWithAccountsAsync(int customerId);
    }
}
