namespace Domain.Models.Dto.General;

public record ModuleDto(
    Guid ModuleId,
    string Title,
    List<LessonDto> Lessons);