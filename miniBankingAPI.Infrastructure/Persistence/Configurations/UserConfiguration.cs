using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miniBankingAPI.Domain.Entities;

namespace miniBankingAPI.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.HasIndex(e => e.Username)
                .IsUnique();
            
            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(e => e.PasswordHash)
                .IsRequired();
            
            builder.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
