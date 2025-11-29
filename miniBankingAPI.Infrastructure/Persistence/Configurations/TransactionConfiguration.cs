using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniBankingAPI.Domain.Entities;

namespace miniBankingAPI.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Amount)
                .HasPrecision(18, 2);
            
            builder.Property(e => e.TransactionType)
                .IsRequired();
            
            builder.Property(e => e.Description)
                .HasMaxLength(500);
            
            // Relationship: Transaction -> Account (Many-to-One)
            builder.HasOne(e => e.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
