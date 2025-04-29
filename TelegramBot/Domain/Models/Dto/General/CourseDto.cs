namespace Domain.Models.Dto.General;

public record CourseDto(
    Guid CourseId,
    string Title,
    string Description,
    List<ModuleDto>? Modules);