using Domain.Entities;
using Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class LessonTypeConfiguration : IEntityTypeConfiguration<LessonTypeEntity>
{
    public void Configure(EntityTypeBuilder<LessonTypeEntity> builder)
    {
        builder.HasData(
            Enum.GetValues<LessonType>()
                .Select(e => new LessonTypeEntity
                {
                    Id = (int)e,
                    Name = e.ToString()
                })
        );
    }
}