namespace Domain.Models.Dto.Admin;

public record CreateModuleDto(
    Guid CourseId,
    string Title
    );