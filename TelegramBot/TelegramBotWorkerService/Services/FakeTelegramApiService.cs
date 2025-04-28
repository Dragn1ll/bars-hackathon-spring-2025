using Domain.Models.Dto.Bot;
using Domain.Models.Dto.General;
using Telegram.Bot;
using TelegramBotWorkerService.Abstractions;

namespace TelegramBotWorkerService.Services;

public class FakeTelegramApiService: ITelegramApiService
{
    private readonly List<CourseDto> _courses = [
    new(1, "Курс 1", "Описание 1"),
    new(2, "Курс 2", "Описание 2")];
    private readonly List<ModuleDto> _modules = [
    new(1, "Модуль 1"),
    new(2, "Модуль 2") ];
    private readonly List<LessonDto> _lessons = [
    new(1, "Урок 1"),
    new(2, "Урок 2")];
    public Task RegisterUser(string phoneNumber, long userId)
    {
        Console.WriteLine($"Registered user {phoneNumber}");
        return Task.CompletedTask;
    }

    public Task<List<CourseDto>> GetCourses()
    {
        return Task.FromResult(_courses);
    }

    public Task<CourseDto> GetCourse(int courseId)
    {
        return Task.FromResult(_courses[courseId]);
    }

    public Task<List<ModuleDto>> GetModules(int courseId)
    {
        return Task.FromResult(_modules);
    }
    
    public Task<ModuleDto> GetModule(int moduleId)
    {
        return Task.FromResult(_modules[moduleId]);
    }

    public Task<List<LessonDto>> GetLessons(int moduleId)
    {
        return Task.FromResult(_lessons);
    }
    
    public Task<LessonDto> GetLesson(int lessonId)
    {
        return Task.FromResult(_lessons[lessonId]);
    }

    public Task<BotQuestionResponseDto> StartTest(int lessonId)
    {
        return Task.FromResult(new BotQuestionResponseDto(1, "Вопрос 1",
            [new BotAnswerResponseDto(1, "Ответ 1"), new BotAnswerResponseDto(2, "Ответ 2")]));
    }

    public Task<BotQuestionResponseDto> SendAnswer(int questionId, string answer)
    {
        return Task.FromResult(new BotQuestionResponseDto(2, "Следующий вопрос",
            [new BotAnswerResponseDto(1, "Ответ 1"), new BotAnswerResponseDto(2, "Ответ 2")]));
    }
}