namespace Domain.Models.Dto.Bot;

public record QuestionDto(
    string QuestionText,
    List<string>? Answers,
    int CorrectAnswerIndex
    );