using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniBankingAPI.Domain.Entities;

namespace miniBankingAPI.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.AccountNumber)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.HasIndex(e => e.AccountNumber)
                .IsUnique();
            
            builder.Property(e => e.Balance)
                .HasPrecision(18, 2);
            
            builder.Property(e => e.CurrencyType)
                .IsRequired();
            
            // Relationship: Account -> Customer (Many-to-One)
            builder.HasOne(e => e.Customer)
                .WithMany(c => c.Accounts)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
