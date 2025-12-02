using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces.IRepositories;
using miniBankingAPI.Infrastructure.Persistence.Data;

namespace miniBankingAPI.Infrastructure.Persistence.Repositories
{
    public class AccountReadRepository : ReadRepository<Account>, IAccountReadRepository
    {
        private new readonly DbSet<Account> _dbSet;

        public AccountReadRepository(BankingDbContext context) : base(context)
        {
            _dbSet = context.Set<Account>();
        }

        public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
            => await _dbSet.SingleOrDefaultAsync(x => x.AccountNumber == accountNumber);

        public Task<List<Account>> GetByCustomerIdAsync(int customerId)
            => _dbSet.Where(x => x.CustomerId == customerId).ToListAsync();

        public Task<Account?> GetWithTransactionsAsync(int accountId)
            => _dbSet.Include(x => x.Transactions).FirstOrDefaultAsync(x => x.Id == accountId);
    }
}
