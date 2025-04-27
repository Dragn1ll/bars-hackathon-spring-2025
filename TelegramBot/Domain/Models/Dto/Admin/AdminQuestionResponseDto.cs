namespace Domain.Models.Dto.Admin;

public record AdminQuestionResponseDto(
    int QuestionId,
    string Question,
    List<AdminQuestionOptionResponseDto> Answers);