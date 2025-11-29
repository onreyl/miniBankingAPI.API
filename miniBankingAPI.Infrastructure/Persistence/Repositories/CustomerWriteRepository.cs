using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces.IRepositories;
using miniBankingAPI.Infrastructure.Persistence.Data;

namespace miniBankingAPI.Infrastructure.Persistence.Repositories
{
    public class CustomerWriteRepository : WriteRepository<Customer>, ICustomerWriteRepository
    {
        private readonly DbSet<Customer> _dbSet;

        public CustomerWriteRepository(BankingDbContext context) : base(context)
        {
            _dbSet = context.Set<Customer>();
        }

        public async Task<bool> IsIdentityNumberExistsAsync(string identityNumber)
            => await _dbSet.AnyAsync(x => x.IdentityNumber == identityNumber);
    }
}
