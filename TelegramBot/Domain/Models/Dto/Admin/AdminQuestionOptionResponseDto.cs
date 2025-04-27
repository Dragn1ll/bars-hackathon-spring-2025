namespace Domain.Models.Dto.Admin;

public record AdminQuestionOptionResponseDto(
    int QuestionOptionId,
    string Answer,
    bool IsCorrect
    );