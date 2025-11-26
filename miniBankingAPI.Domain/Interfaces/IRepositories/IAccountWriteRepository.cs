using miniBankingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Domain.Interfaces.IRepositories
{
    public interface IAccountWriteRepository : IWriteRepository<Account>
    {
        Task<bool> IsAccountNumberExistsAsync(string accountNumber);
        Task DeactivateAccountAsync(int accountId);
    }
}
