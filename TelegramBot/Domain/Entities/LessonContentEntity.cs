using Domain.Models.Enums;

namespace Domain.Entities;

public class LessonContentEntity
{
    public Guid LessonContentId { get; set; }
    public Guid LessonId { get; set; }
    public LessonEntity Lesson { get; set; }
    public LessonContentType Type { get; set; }
    public string? FileName { get; set; }
    public string? TextContent { get; set; }
    public bool IsDeleted { get; set; }
}