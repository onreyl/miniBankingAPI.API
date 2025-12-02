using miniBankingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace miniBankingAPI.Domain.Interfaces
{
    public interface IReadRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    }
}
