namespace Domain.Entities;

public class UserCompletedLessonEntity
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public Admin User { get; set; }
    
    public Guid LessonId { get; set; }
    public LessonEntity Lesson { get; set; }
    
    public bool IsSuccessful { get; set; }
}