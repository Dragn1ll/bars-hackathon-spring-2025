namespace Domain.Models.Dto.Admin;

public record CreateQuestionDto(
    int LessonId,
    string Question,
    List<string> Answers,
    int CorrectAnswerIndex
    );