namespace Persistence.DataAccess.Entities;

public class LessonEntity
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public ModuleEntity Module { get; set; }
    public string Title { get; set; }
    public string ContentType { get; set; }
    public ICollection<LessonContentEntity> LessonContents { get; set; }
    public ICollection<QuizQuestionEntity> QuizQuestions { get; set; }
}