namespace Domain.Models.Dto.Bot;

public record BotAnswerResponseDto(
    Guid AnswerId,
    string Text
    );