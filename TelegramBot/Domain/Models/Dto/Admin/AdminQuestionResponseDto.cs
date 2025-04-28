namespace Domain.Models.Dto.Admin;

public record AdminQuestionResponseDto(
    Guid QuestionId,
    string Question,
    List<AdminQuestionOptionResponseDto> Answers);