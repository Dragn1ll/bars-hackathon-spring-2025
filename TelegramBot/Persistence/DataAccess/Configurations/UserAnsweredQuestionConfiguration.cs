using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class UserAnsweredQuestionConfiguration : IEntityTypeConfiguration<UserAnsweredQuestionEntity>
{
    public void Configure(EntityTypeBuilder<UserAnsweredQuestionEntity> builder)
    {
        builder.HasKey(x => new { x.QuestionId, x.UserId });
        
        builder.HasOne(uaq => uaq.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(uaq => uaq.QuestionId);
        
        builder.HasOne(uaq => uaq.User)
            .WithMany(q => q.Answers)
            .HasForeignKey(uaq => uaq.UserId);
    }
}