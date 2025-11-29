using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces;
using miniBankingAPI.Infrastructure.Persistence.Data;

namespace miniBankingAPI.Infrastructure.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        protected readonly BankingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public WriteRepository(BankingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
