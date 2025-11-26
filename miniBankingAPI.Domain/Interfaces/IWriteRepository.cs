using miniBankingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Domain.Interfaces
{
    public interface IWriteRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
