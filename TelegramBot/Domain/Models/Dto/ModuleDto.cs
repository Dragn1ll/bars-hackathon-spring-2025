namespace Domain.Models.Dto;

public record ModuleDto(
    string Title,
    string Description,
    List<LessonDto>? Lessons);