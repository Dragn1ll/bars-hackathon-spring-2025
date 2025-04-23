using Domain.Entities;
using Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess.Configurations;

public class LessonContentTypeConfiguration : IEntityTypeConfiguration<LessonContentTypeEntity>
{
    public void Configure(EntityTypeBuilder<LessonContentTypeEntity> builder)
    {
        builder.HasData(
            Enum.GetValues<LessonContentType>()
                .Select(e => new LessonContentTypeEntity
                {
                    Id = (int)e,
                    Name = e.ToString()
                })
        );
    }
}