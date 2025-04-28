using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
{
    public void Configure(EntityTypeBuilder<AdminEntity> builder)
    {
        builder.ToTable("admins");
        
        builder.HasKey(a => a.AdminId);
        
        builder.Property(a => a.Username)
            .HasMaxLength(100);
        builder.HasIndex(a => a.Username)
            .IsUnique();

        builder.Property(a => a.PasswordHash)
            .HasMaxLength(100);
    }
}