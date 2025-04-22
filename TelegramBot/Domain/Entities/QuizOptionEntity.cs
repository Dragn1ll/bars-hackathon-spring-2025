namespace Domain.Entities;

public class QuizOptionEntity
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public QuizQuestionEntity QuizQuestion { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
}