using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class UserCompletedLessonConfiguration : IEntityTypeConfiguration<UserCompletedLessonEntity>
{
    public void Configure(EntityTypeBuilder<UserCompletedLessonEntity> builder)
    {
        builder.HasKey(ul => ul.Id);
        
        builder.HasOne(ul => ul.User)
            .WithMany(u => u.CompletedLessons)
            .HasForeignKey(ul => ul.UserId);
        
        builder.HasOne(ul => ul.Lesson)
            .WithMany(l => l.CompletedByUsers)
            .HasForeignKey(ul => ul.LessonId);
    }
}