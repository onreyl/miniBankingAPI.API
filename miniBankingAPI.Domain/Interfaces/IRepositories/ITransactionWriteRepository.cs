using miniBankingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniBankingAPI.Domain.Interfaces.IRepositories
{
    public interface ITransactionWriteRepository : IWriteRepository<Transaction>
    {
        
    }
}
