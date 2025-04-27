namespace Domain.Models.Dto.Bot;

public record UserAnswerDtoRequest(
    long UserId,
    Guid QuestionId,
    Guid AnswerId
    );