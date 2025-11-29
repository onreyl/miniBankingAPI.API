using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniBankingAPI.Domain.Entities;

namespace miniBankingAPI.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(e => e.IdentityNumber)
                .IsRequired()
                .HasMaxLength(11);
            
            builder.HasIndex(e => e.IdentityNumber)
                .IsUnique();
            
            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(e => e.Phone)
                .HasMaxLength(15);
        }
    }
}
