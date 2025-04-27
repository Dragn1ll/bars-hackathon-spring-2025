using Domain.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.Models.Dto.Admin;

public record AddLessonContentDto(
    int LessonId,
    LessonContentType ContentType,
    IFormFile File
    );