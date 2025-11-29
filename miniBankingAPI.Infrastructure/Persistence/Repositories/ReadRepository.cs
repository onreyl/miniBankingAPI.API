using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces;
using miniBankingAPI.Infrastructure.Persistence.Data;
using System.Linq.Expressions;

namespace miniBankingAPI.Infrastructure.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        protected readonly BankingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public ReadRepository(BankingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.FirstOrDefaultAsync(predicate);

        public IQueryable<T> GetAll()
            => _dbSet;

        public async Task<T> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);
    }
}
