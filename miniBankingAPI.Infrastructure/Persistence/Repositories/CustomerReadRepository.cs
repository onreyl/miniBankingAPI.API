using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces.IRepositories;
using miniBankingAPI.Infrastructure.Persistence.Data;

namespace miniBankingAPI.Infrastructure.Persistence.Repositories
{
    public class CustomerReadRepository : ReadRepository<Customer>, ICustomerReadRepository
    {
        private readonly DbSet<Customer> _dbSet;

        public CustomerReadRepository(BankingDbContext context) : base(context)
        {
            _dbSet = context.Set<Customer>();
        }

        public async Task<Customer> GetByIdentityNumberAsync(string identityNumber)
            => await _dbSet.FirstOrDefaultAsync(x => x.IdentityNumber == identityNumber);

        public async Task<Customer> GetWithAccountsAsync(int customerId)
            => await _dbSet.Include(x => x.Accounts).FirstOrDefaultAsync(x => x.Id == customerId);
    }
}
