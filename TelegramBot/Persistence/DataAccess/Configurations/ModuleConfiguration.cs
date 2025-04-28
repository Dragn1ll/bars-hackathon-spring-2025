using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<ModuleEntity>
{
    public void Configure(EntityTypeBuilder<ModuleEntity> builder)
    {
        builder.ToTable("modules");
        
        builder.HasKey(m => m.ModuleId);
        
        builder.Property(m => m.Title)
            .HasMaxLength(255);
        
        builder.HasOne(m => m.Course)
            .WithMany(c => c.Modules)
            .HasForeignKey(m => m.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false);
    }
}