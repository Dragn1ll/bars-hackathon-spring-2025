namespace Domain.Models.Dto.General;

public record ModuleDto(
    Guid ModuleId,
    Guid CourseId,
    string Title,
    List<LessonDto>? Lessons);