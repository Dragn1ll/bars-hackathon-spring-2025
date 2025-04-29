namespace Domain.Models.Dto.Admin;

public record CreateQuestionDto(
    Guid LessonId,
    string QuestionText,
    List<CreateQuestionOptionDto> Answers
    );