using Domain.Models;
using Domain.Models.Enums;

namespace Domain.Entities;

public class LessonContentEntity
{
    public int LessonId { get; set; }
    public LessonEntity Lesson { get; set; }
    public LessonContentType Type { get; set; }
    public string Content { get; set; }
}