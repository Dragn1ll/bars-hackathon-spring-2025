namespace Domain.Entities;

public class QuizQuestionEntity
{
    public Guid QuestionId { get; set; }
    public Guid LessonId { get; set; }
    public LessonEntity Lesson { get; set; }
    public string QuestionText { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<QuizOptionEntity> QuizOptions { get; set; }
    public IEnumerable<UserAnsweredQuestionEntity>? Answers { get; set; }
}