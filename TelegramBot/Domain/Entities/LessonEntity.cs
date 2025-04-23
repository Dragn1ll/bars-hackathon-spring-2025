namespace Domain.Entities;

public class LessonEntity
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public ModuleEntity Module { get; set; }
    public string Title { get; set; }
    public int LessonTypeId { get; set; }
    public LessonTypeEntity LessonType { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<LessonContentEntity> LessonContents { get; set; }
    public ICollection<QuizQuestionEntity> QuizQuestions { get; set; }
    public ICollection<UserCompletedLessonEntity> CompletedByUsers { get; set; }
}