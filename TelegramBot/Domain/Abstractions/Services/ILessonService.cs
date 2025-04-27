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
    Task<Result> DeleteLesson(int lessonId);
    Task<Result<List<LessonDto>>> GetAllLessons(int moduleId);
    Task<Result<List<Stream>>> GetAllLessonFiles(int lessonId);
}