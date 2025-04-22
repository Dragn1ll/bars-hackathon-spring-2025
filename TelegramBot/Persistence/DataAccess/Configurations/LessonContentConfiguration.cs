using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class LessonContentConfiguration : IEntityTypeConfiguration<LessonContentEntity>
{
    public void Configure(EntityTypeBuilder<LessonContentEntity> builder)
    {
        builder.ToTable("lesson_contents");
        builder.HasKey(lc => lc.LessonId);
        builder.Property(lc => lc.Content).IsRequired();
        builder.HasOne(lc => lc.Lesson)
            .WithMany(l => l.LessonContents)
            .HasForeignKey(lc => lc.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}