using miniBankingAPI.Domain.Entities;
using miniBankingAPI.Domain.Interfaces.IRepositories;
using miniBankingAPI.Infrastructure.Persistence.Data;

namespace miniBankingAPI.Infrastructure.Persistence.Repositories
{
    public class TransactionWriteRepository : WriteRepository<Transaction>, ITransactionWriteRepository
    {
        public TransactionWriteRepository(BankingDbContext context) : base(context)
        {
        }

        // Transaction için özel write metodu gerekirse buraya eklenebilir
    }
}
