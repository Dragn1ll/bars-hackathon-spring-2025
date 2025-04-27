namespace Domain.Models.Dto.Bot;

public record CourseDto(
    string Title,
    string Description,
    List<ModuleDto>? Modules);