namespace Domain.Entities;

public class QuizOptionEntity
{
    public Guid OptionId { get; set; }
    public Guid QuestionId { get; set; }
    public QuizQuestionEntity QuizQuestion { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsDeleted { get; set; }
}