namespace Domain.Entities;

public class QuizQuestionEntity
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public LessonEntity Lesson { get; set; }
    public string Question { get; set; }
    public ICollection<QuizOptionEntity> QuizOptions { get; set; }
}