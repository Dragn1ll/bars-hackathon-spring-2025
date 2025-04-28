using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class QuizOptionConfiguration : IEntityTypeConfiguration<QuizOptionEntity>
{
    public void Configure(EntityTypeBuilder<QuizOptionEntity> builder)
    {
        builder.ToTable("quiz_options");
        
        builder.HasKey(qo => qo.OptionId);
        
        builder.Property(qo => qo.Text);
        
        builder.Property(qo => qo.IsCorrect);
        
        builder.HasOne(qo => qo.QuizQuestion)
            .WithMany(q => q.QuizOptions)
            .HasForeignKey(qo => qo.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(qo => qo.IsDeleted)
            .HasDefaultValue(false);
    }
}