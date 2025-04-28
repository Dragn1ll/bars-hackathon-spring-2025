using Domain.Entities;
using Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(u => u.UserId);
        
        builder.Property(u => u.Email)
            .HasMaxLength(100);
        
        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique();
        
        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false);
    }
}