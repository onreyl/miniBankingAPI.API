using Microsoft.EntityFrameworkCore;
using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces;
using miniBankingAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Infrastructure.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        protected readonly BankingDbContext _context;
        protected readonly DbSet<T> _dbset;
        public WriteRepository(BankingDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }
        public async Task AddAsync(T entity)
            => await _dbset.AddAsync(entity);

        public void Update(T entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _dbset.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }
    }
}
