namespace Domain.Models.Dto.Bot;

public record UserAnswerDtoRequest(
    long UserId,
    int QuestionId,
    int AnswerId
    );