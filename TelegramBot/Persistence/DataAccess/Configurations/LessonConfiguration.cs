using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<LessonEntity>
{
    public void Configure(EntityTypeBuilder<LessonEntity> builder)
    {
        builder.ToTable("lessons");
        
        builder.HasKey(l => l.LessonId);
        
        builder.Property(l => l.Title)
            .HasMaxLength(255);
        
        builder.HasOne(l => l.Module)
            .WithMany(m => m.Lessons)
            .HasForeignKey(l => l.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(с => с.IsDeleted)
            .HasDefaultValue(false);
    }
}