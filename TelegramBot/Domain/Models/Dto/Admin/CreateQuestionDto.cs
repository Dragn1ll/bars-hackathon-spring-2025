namespace Domain.Models.Dto.Admin;

public record CreateQuestionDto(
    Guid LessonId,
    string Question,
    List<CreateQuestionOptionDto> Answers
    );