namespace Domain.Models.Dto.Bot;

public record CourseDto(
    int CourseId,
    string Title,
    string Description,
    List<ModuleDto>? Modules);