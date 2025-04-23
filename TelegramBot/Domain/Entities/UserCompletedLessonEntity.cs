namespace Domain.Entities;

public class UserCompletedLessonEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    
    public int LessonId { get; set; }
    public LessonEntity Lesson { get; set; }
    
    public bool IsSuccessful { get; set; }
    public double? Score { get; set; }
}