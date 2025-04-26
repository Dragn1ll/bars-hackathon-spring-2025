namespace Domain.Models.Dto;

public record QuestionDto(
    string QuestionText,
    List<string>? Answers,
    int CorrectAnswerIndex);