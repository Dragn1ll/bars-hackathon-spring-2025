using Domain.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.Models.Dto.Admin;

public record CreateLessonContentDto(
    Guid LessonId,
    string FileName);