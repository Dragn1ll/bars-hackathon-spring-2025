using Domain.Models.Dto.Bot;
using Domain.Models.Dto.General;
using TelegramBotWorkerService.Abstractions;

namespace TelegramBotWorkerService.Services;

public class FakeTelegramApiService : ITelegramApiService
{
    private static readonly List<LessonDto> Lessons = [
        new(Guid.NewGuid(),Guid.NewGuid(), "Урок 1"),
        new(Guid.NewGuid(),Guid.NewGuid(), "Урок 2")];
    private static readonly List<ModuleDto> Modules = [
        new(Lessons[0].ModuleId, Guid.NewGuid(),"Модуль 1", Lessons),
        new(Lessons[1].ModuleId, Guid.NewGuid(),"Модуль 2", Lessons), ];
    private static readonly List<CourseDto> Courses = [
    new(Modules[0].LessonId, "Курс 1", "Описание 1", Modules),
    new(Modules[0].LessonId, "Курс 2", "Описание 2", Modules)];
    
    
    public Task RegisterUser(string phoneNumber, long userId)
    {
        Console.WriteLine($"Registered user {phoneNumber}");
        return Task.CompletedTask;
    }

    public Task<List<CourseDto>> GetCourses()
    {
        return Task.FromResult(Courses);
    }

    public Task<CourseDto?> GetCourse(Guid courseId)
    {
        return Task.FromResult(Courses.FirstOrDefault(c => c.CourseId == courseId));
    }
    
    public Task<ModuleDto?> GetModule(Guid moduleId)
    {
        return Task.FromResult(Modules.FirstOrDefault(m => m.ModuleId == moduleId));
    }
    
    public Task<LessonDto?> GetLesson(Guid lessonId)
    {
        return Task.FromResult(Lessons.FirstOrDefault(l => l.LessonId == lessonId));
    }

    public Task<BotQuestionResponseDto> StartTest(Guid lessonId)
    {
        return Task.FromResult(new BotQuestionResponseDto(Guid.NewGuid(), "Вопрос 1",
            [new BotAnswerResponseDto(Guid.NewGuid(), "Ответ 1"), new BotAnswerResponseDto(Guid.NewGuid(), "Ответ 2")]));
    }

    public Task<BotQuestionResponseDto> SendAnswer(Guid questionId, string answer)
    {
        return Task.FromResult(new BotQuestionResponseDto(Guid.NewGuid(), "Следующий вопрос",
            [new BotAnswerResponseDto(Guid.NewGuid(), "Ответ 1"), new BotAnswerResponseDto(Guid.NewGuid(), "Ответ 2")]));
    }
}