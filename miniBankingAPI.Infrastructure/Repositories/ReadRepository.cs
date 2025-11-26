using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces;
using miniBankingAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace miniBankingAPI.Infrastructure.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        protected readonly BankingDbContext _context;
        protected readonly DbSet<T> _dbset;
        public ReadRepository(BankingDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            => await _dbset.FirstOrDefaultAsync(predicate);

        public IQueryable<T> GetAll()
            => _dbset;

        public async Task<T> GetByIdAsync(int id)
            => await _dbset.FindAsync(id);
    }
}
