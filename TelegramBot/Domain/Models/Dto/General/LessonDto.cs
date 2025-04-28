namespace Domain.Models.Dto.General;

public record LessonDto(
    Guid LessonId,
    Guid ModuleId,
    string Title);