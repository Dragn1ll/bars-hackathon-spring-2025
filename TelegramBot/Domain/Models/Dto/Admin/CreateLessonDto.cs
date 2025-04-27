using Domain.Models.Enums;

namespace Domain.Models.Dto.Admin;

public record CreateLessonDto(
    int ModuleId,
    string Title,
    LessonType Type
    );