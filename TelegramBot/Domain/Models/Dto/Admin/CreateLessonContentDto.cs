using Domain.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.Models.Dto.Admin;

public record CreateLessonContentDto(
    int LessonId,
    string FileName,
    LessonContentType ContentType);