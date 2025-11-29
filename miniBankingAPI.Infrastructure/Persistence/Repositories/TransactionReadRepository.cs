using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces.IRepositories;
using miniBankingAPI.Infrastructure.Persistence.Data;

namespace miniBankingAPI.Infrastructure.Persistence.Repositories
{
    public class TransactionReadRepository : ReadRepository<Transaction>, ITransactionReadRepository
    {
        private readonly DbSet<Transaction> _dbSet;

        public TransactionReadRepository(BankingDbContext context) : base(context)
        {
            _dbSet = context.Set<Transaction>();
        }

        public async Task<List<Transaction>> GetByAccountIdAsync(int accountId)
            => await _dbSet.Where(x => x.AccountId == accountId)
                          .OrderByDescending(x => x.CreatedDate)
                          .ToListAsync();

        public async Task<List<Transaction>> GetByDateRangeAsync(int accountId, DateTime startDate, DateTime endDate)
            => await _dbSet.Where(x => x.AccountId == accountId && 
                                      x.CreatedDate >= startDate && 
                                      x.CreatedDate <= endDate)
                          .OrderByDescending(x => x.CreatedDate)
                          .ToListAsync();
    }
}
