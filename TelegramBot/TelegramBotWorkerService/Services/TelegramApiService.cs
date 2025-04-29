using System.Net.Http.Json;
using System.Text.Json;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.Bot;
using Domain.Models.Dto.General;
using TelegramBotWorkerService.Abstractions;

namespace TelegramBotWorkerService.Services;

public class TelegramApiService(HttpClient client): ITelegramApiService
{
    public async Task RegisterUser(string phoneNumber, long? userId)
    {
        var registerUserDto = new RegisterUserDto(userId ?? 0, phoneNumber);
        var result = await client.PostAsJsonAsync("/users/register", registerUserDto);
        if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            throw new HttpRequestException("Could not register user, please try again");
    }

    public async Task<List<CourseDto>?> GetCourses(long userId)
    {
        return await GetAsync<List<CourseDto>>("courses", userId);
    }

    public async Task<CourseDto?> GetCourse(long userId, Guid courseId)
    {
        return await GetAsync<CourseDto>($"courses/{courseId}", userId);
    }

    public async Task<ModuleDto?> GetModule(long userId, Guid moduleId)
    {
        return await GetAsync<ModuleDto>($"module/{moduleId}", userId);
    }

    public async Task<LessonDto?> GetLesson(long userId, Guid lessonId)
    {
        return await GetAsync<LessonDto>($"lessons/{lessonId}", userId);
    }

    public async Task<List<string>?> GetLessonContent(long userId, Guid lessonId)
    {
        return await GetAsync<List<string>>($"lessons/files/urls/{lessonId}", userId);
    }

    public async Task<BotQuestionResponseDto?> StartTest(long userId, Guid lessonId)
    {
        return await GetAsync<BotQuestionResponseDto>($"quiz/questions/{lessonId}/{userId}", userId);
    }

    public async Task<BotQuestionResponseDto?> SendAnswer(long userId, Guid answerId, Guid? questionId = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "next");
        request.Headers.Add("X-User-Id", userId.ToString());
        request.Content = new StringContent(
            JsonSerializer.Serialize(
                new UserAnswerDtoRequest(userId, questionId, answerId)));
        var result = await client.SendAsync(request);
        switch (result.StatusCode)
        {
            case System.Net.HttpStatusCode.InternalServerError:
                throw new HttpRequestException("Server error, please try again");
            case System.Net.HttpStatusCode.Unauthorized:
                throw new UnauthorizedAccessException("You are not authorized to do it");
            case System.Net.HttpStatusCode.NotFound:
                throw new KeyNotFoundException("Content was not found");
            default:
            {
                return await result.Content.ReadFromJsonAsync<BotQuestionResponseDto>();
            }
        }
    }

    private async Task<T?> GetAsync<T>(string url, long userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("x-user-id", userId.ToString());
        var result = await client.SendAsync(request);
        switch (result.StatusCode)
        {
            case System.Net.HttpStatusCode.InternalServerError:
                throw new HttpRequestException("Server error, please try again");
            case System.Net.HttpStatusCode.Unauthorized:
                throw new UnauthorizedAccessException("You are not authorized to do it");
            case System.Net.HttpStatusCode.NotFound:
                throw new KeyNotFoundException("Content was not found");
            default:
            {
                return await result.Content.ReadFromJsonAsync<T>();
            }
        }
    }
}