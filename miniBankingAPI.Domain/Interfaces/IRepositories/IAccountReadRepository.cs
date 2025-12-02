using miniBankingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Domain.Interfaces.IRepositories
{
    public interface IAccountReadRepository : IReadRepository<Account> 
    {
        Task<Account?> GetByAccountNumberAsync(string accountNumber);
        Task<List<Account>> GetByCustomerIdAsync(int customerId);
        Task<Account?> GetWithTransactionsAsync(int accountId);
    }
}
