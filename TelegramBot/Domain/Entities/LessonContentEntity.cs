namespace Domain.Entities;

public class LessonContentEntity
{
    public Guid LessonContentId { get; set; }
    public Guid LessonId { get; set; }
    public LessonEntity Lesson { get; set; }
    public int LessonContentTypeId { get; set; }
    public LessonContentTypeEntity Type { get; set; }
    public string? FileName { get; set; }
    public string? TextContent { get; set; }
    public bool IsDeleted { get; set; }
}