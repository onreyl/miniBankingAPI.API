using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces.IRepositories;
using miniBankingAPI.Infrastructure.Persistence.Data;

namespace miniBankingAPI.Infrastructure.Persistence.Repositories
{
    public class AccountWriteRepository : WriteRepository<Account>, IAccountWriteRepository
    {
        private readonly DbSet<Account> _dbSet;

        public AccountWriteRepository(BankingDbContext context) : base(context)
        {
            _dbSet = context.Set<Account>();
        }

        public async Task<bool> IsAccountNumberExistsAsync(string accountNumber)
            => await _dbSet.AnyAsync(x => x.AccountNumber == accountNumber);

        public async Task DeactivateAccountAsync(int accountId)
        {
            var account = await _dbSet.FindAsync(accountId);
            if(account != null)
            {
                account.IsActive = false;
                account.UpdatedDate = DateTime.UtcNow;
                _dbSet.Update(account);
            }
        }
    }
}
