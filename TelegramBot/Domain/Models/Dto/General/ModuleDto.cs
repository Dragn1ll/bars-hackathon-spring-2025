namespace Domain.Models.Dto.General;

public record ModuleDto(
    Guid ModuleId,
    Guid LessonId,
    string Title,
    List<LessonDto> Lessons);