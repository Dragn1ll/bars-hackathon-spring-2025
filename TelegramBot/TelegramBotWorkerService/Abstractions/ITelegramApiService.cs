using Domain.Models.Dto;
using Domain.Models.Dto.Bot;
using Domain.Models.Dto.General;

namespace TelegramBotWorkerService.Abstractions;

public interface ITelegramApiService
{
    public Task RegisterUser(string phoneNumber, long userId);
    public Task<List<CourseDto>> GetCourses();
    public Task<CourseDto> GetCourse(int courseId);
    public Task<List<ModuleDto>> GetModules(int courseId);
    public Task<List<LessonDto>> GetLessons(int moduleId);
    public Task<BotQuestionResponseDto> StartTest(int lessonId);
    public Task<BotQuestionResponseDto> SendAnswer(int questionId, string answer);
}