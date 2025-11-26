using miniBankingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Domain.Interfaces.IRepositories
{
    public interface ITransactionReadRepository : IReadRepository<Transaction>
    {
        Task<List<Transaction>> GetByAccountIdAsync(int accountId);
        Task<List<Transaction>> GetByDateRangeAsync(int accountId, DateTime startDate, DateTime endDate);
    }
}
