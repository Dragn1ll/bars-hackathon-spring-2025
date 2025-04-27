using Domain.Models.Enums;

namespace Domain.Models.Dto.Admin;

public record CreateLessonDto(
    Guid ModuleId,
    string Title);