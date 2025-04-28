using Domain.Models.Dto;
using Domain.Models.Dto.Bot;
using Domain.Models.Dto.General;

namespace TelegramBotWorkerService.Abstractions;

public interface ITelegramApiService
{
    Task RegisterUser(string phoneNumber, long userId);
    Task<List<CourseDto>> GetCourses();
    Task<CourseDto?> GetCourse(Guid courseId);
    Task<ModuleDto?> GetModule(Guid moduleId);
    Task<LessonDto?> GetLesson(Guid lessonId);
    Task<BotQuestionResponseDto> StartTest(Guid lessonId);
    Task<BotQuestionResponseDto> SendAnswer(Guid questionId, string answer);
}