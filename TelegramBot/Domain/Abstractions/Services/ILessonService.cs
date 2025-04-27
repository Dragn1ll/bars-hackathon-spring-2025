using Domain.Entities;
using Domain.Models.Enums;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface ILessonService
{
    Task<Result<LessonEntity>> CreateLesson(LessonEntity lesson);
    Task<Result<LessonEntity>> UpdateLesson(LessonEntity lesson);
    Task<Result> DeleteLesson(int lessonId);
    Task<Result<List<LessonEntity>>> GetAllLessons(int moduleId);
    Task<Result<LessonEntity>> GetLessonWithContentAndQuizzes(int lessonId);
}