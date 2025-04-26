namespace Domain.Models.Dto;

public record CourseDto(
    string Title,
    string Description,
    List<ModuleDto>? Modules);