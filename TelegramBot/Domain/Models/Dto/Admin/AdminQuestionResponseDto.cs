namespace Domain.Models.Dto.Admin;

public record AdminQuestionResponseDto(
    Guid QuestionId,
    string QuestionText,
    List<AdminQuestionOptionResponseDto> Answers);