namespace Domain.Models.Dto.Admin;

public record AdminQuestionOptionResponseDto(
    Guid QuestionOptionId,
    string Answer,
    bool IsCorrect
    );