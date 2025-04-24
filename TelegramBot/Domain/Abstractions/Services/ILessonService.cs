using Domain.Entities;
using Domain.Models.Enums;

namespace Domain.Abstractions.Services;

public interface ILessonService
{
    Task<LessonEntity> CreateLesson(LessonEntity lesson);
    Task<LessonEntity> UpdateLesson(LessonEntity lesson);
    Task<bool> DeleteLesson(int lessonId);
    Task<List<LessonEntity>> GetAllLessons(int moduleId);
    Task<LessonEntity> GetLessonWithContentAndQuizzes(int lessonId);
}