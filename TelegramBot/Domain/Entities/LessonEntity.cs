namespace Domain.Entities;

public class LessonEntity
{
    public Guid LessonId { get; set; }
    public Guid ModuleId { get; set; }
    public ModuleEntity Module { get; set; }
    public string Title { get; set; }
    public bool IsDeleted { get; set; }
    public int Order { get; set; }
    public ICollection<LessonContentEntity> LessonContents { get; set; }
    public ICollection<QuizQuestionEntity> QuizQuestions { get; set; }
    public ICollection<UserCompletedLessonEntity> CompletedByUsers { get; set; }
}