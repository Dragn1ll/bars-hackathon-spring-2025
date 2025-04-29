using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Domain.Models.Enums;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface ILessonService
{
    Task<Result<LessonDto>> CreateLesson(CreateLessonDto lesson);
    Task<Result<LessonDto>> UpdateLesson(LessonDto lesson);
    Task<Result> DeleteLesson(Guid lessonId);
    Task<Result<List<LessonDto>>> GetAllLessons(Guid moduleId);
    Task<Result<List<byte[]>>> GetAllLessonFiles(Guid lessonId);
    Task<Result<List<string>>> GetLessonFilesUrls(Guid lessonId);
    Task<Result<LessonDto>> GetLesson(Guid lessonId);
}