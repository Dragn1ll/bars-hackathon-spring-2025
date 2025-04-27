namespace Domain.Models.Dto.Bot;

public record BotQuestionResponseDto(
    int QuestionId,
    string QuestionText,
    List<BotAnswerResponseDto> Answers
    );