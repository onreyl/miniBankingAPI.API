using miniBankingAPI.Domain.Interfaces;
using miniBankingAPI.Domain.Interfaces.IRepositories;
using miniBankingAPI.Infrastructure.Persistence.Data;

namespace miniBankingAPI.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankingDbContext _context;

        public IAccountReadRepository AccountsRead { get; }
        public IAccountWriteRepository AccountsWrite { get; }
        public ICustomerReadRepository CustomersRead { get; }
        public ICustomerWriteRepository CustomersWrite { get; }
        public ITransactionReadRepository TransactionsRead { get; }
        public ITransactionWriteRepository TransactionsWrite { get; }

        public UnitOfWork(BankingDbContext context)
        {
            _context = context;
           
            AccountsRead = new AccountReadRepository(context);
            AccountsWrite = new AccountWriteRepository(context);
            CustomersRead = new CustomerReadRepository(context);
            CustomersWrite = new CustomerWriteRepository(context);
            TransactionsRead = new TransactionReadRepository(context);
            TransactionsWrite = new TransactionWriteRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
