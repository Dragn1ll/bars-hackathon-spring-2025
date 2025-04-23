namespace Domain.Entities;

public class LessonContentEntity
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public LessonEntity Lesson { get; set; }
    public int LessonContentTypeId { get; set; }
    public LessonContentTypeEntity Type { get; set; }
    public string Content { get; set; }
    public bool IsDeleted { get; set; }
}