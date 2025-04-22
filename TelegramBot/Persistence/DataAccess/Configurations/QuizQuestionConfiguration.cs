using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestionEntity>
{
    public void Configure(EntityTypeBuilder<QuizQuestionEntity> builder)
    {
        builder.ToTable("quiz_questions");
        
        builder.HasKey(q => q.Id);
        
        builder.Property(q => q.Question)
            .IsRequired();
        
        builder.HasOne(q => q.Lesson)
            .WithMany(l => l.QuizQuestions)
            .HasForeignKey(q => q.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}