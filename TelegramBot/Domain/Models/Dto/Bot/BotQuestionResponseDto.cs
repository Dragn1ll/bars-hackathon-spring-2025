namespace Domain.Models.Dto.Bot;

public record BotQuestionResponseDto(
    Guid QuestionId,
    string QuestionText,
    List<BotAnswerResponseDto> Answers
    );