namespace Persistence.DataAccess.Entities;

public class LessonContentEntity
{
    public int LessonId { get; set; }
    public LessonEntity Lesson { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
}