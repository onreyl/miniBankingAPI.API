using miniBankingAPI.Domain.Interfaces.IRepositories;
using System;

namespace miniBankingAPI.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        // Account repositories
        IAccountReadRepository AccountsRead { get; }
        IAccountWriteRepository AccountsWrite { get; }
        
        // Customer repositories
        ICustomerReadRepository CustomersRead { get; }
        ICustomerWriteRepository CustomersWrite { get; }
        
        // Transaction repositories
        ITransactionReadRepository TransactionsRead { get; }
        ITransactionWriteRepository TransactionsWrite { get; }
        
        Task<int> SaveChangesAsync();
    }
}
