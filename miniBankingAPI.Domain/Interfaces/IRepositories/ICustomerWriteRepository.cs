using miniBankingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Domain.Interfaces.IRepositories
{
    public interface ICustomerWriteRepository : IWriteRepository<Customer>
    {
        Task<bool> IsIdentityNumberExistsAsync(string identityNumber);
    }
}
