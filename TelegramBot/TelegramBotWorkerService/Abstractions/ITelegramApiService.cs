using Domain.Models.Dto;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.Bot;
using Domain.Models.Dto.General;

namespace TelegramBotWorkerService.Abstractions;

public interface ITelegramApiService
{
    Task RegisterUser(string phoneNumber, long? userId);
    Task<List<CourseDto>?> GetCourses(long userId);
    Task<CourseDto?> GetCourse(long userId, Guid courseId);
    Task<ModuleDto?> GetModule(long userId, Guid moduleId);
    Task<LessonDto?> GetLesson(long userId, Guid lessonId);
    Task<List<string>?> GetLessonContent(long userId, Guid lessonId);
    Task<BotQuestionResponseDto?> StartTest(long userId, Guid lessonId);
    public Task<BotQuestionResponseDto?> SendAnswer(long userId, Guid answerId, Guid? questionId = null);
}