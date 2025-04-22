using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class QuizOptionConfiguration : IEntityTypeConfiguration<QuizOptionEntity>
{
    public void Configure(EntityTypeBuilder<QuizOptionEntity> builder)
    {
        builder.ToTable("quiz_options");
        
        builder.HasKey(qo => qo.Id);
        
        builder.Property(qo => qo.Text)
            .IsRequired();
        
        builder.Property(qo => qo.IsCorrect)
            .IsRequired();
        
        builder.HasOne(qo => qo.QuizQuestion)
            .WithMany(q => q.QuizOptions)
            .HasForeignKey(qo => qo.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}