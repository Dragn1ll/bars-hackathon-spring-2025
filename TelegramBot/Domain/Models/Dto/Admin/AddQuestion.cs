namespace Domain.Models.Dto.Admin;

public record AddQuestion(
    int LessonId,
    string Question,
    List<string> Answers,
    int CorrectAnswerIndex
    );