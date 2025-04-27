namespace Domain.Models.Dto.Bot;

public record ModuleDto(
    string Title,
    string Description,
    List<LessonDto>? Lessons);