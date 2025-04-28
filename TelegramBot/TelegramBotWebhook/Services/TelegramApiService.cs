using Domain.Models.Dto.Bot;
using Domain.Models.Dto.General;
using TelegramBotWebhook.Abstractions;

namespace TelegramBotWebhook.Services;

public class TelegramApiService : ITelegramApiService
{
    public Task RegisterUser(string phoneNumber, long userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CourseDto>> GetCourses()
    {
        throw new NotImplementedException();
    }

    public Task<CourseDto?> GetCourse(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<ModuleDto?> GetModule(Guid moduleId)
    {
        throw new NotImplementedException();
    }

    public Task<LessonDto?> GetLesson(Guid lessonId)
    {
        throw new NotImplementedException();
    }

    public Task<BotQuestionResponseDto> StartTest(Guid lessonId)
    {
        throw new NotImplementedException();
    }

    public Task<BotQuestionResponseDto> SendAnswer(Guid questionId, string answer)
    {
        throw new NotImplementedException();
    }
}