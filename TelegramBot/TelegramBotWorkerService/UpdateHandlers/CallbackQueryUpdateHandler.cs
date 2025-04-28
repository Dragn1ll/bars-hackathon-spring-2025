using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotWorkerService.Abstractions;
using TelegramBotWorkerService.Models;

namespace TelegramBotWorkerService.UpdateHandlers;

public class CallbackQueryUpdateHandler: ICustomUpdateHandler
{
    private readonly ITelegramApiService _apiService;
    private readonly Dictionary<string, Func<ITelegramBotClient, CallbackQuery, CancellationToken, Task>> _messageRoutes;
    public CallbackQueryUpdateHandler(ITelegramApiService apiService)
    {
        _apiService = apiService;
        _messageRoutes = GetType()
            .GetMethods()
            .Where(m => m.IsDefined(typeof(CommandAttribute), false))
            .ToDictionary<MethodInfo, string, Func<ITelegramBotClient, CallbackQuery, CancellationToken, Task>>(
                method => (method.GetCustomAttribute<CommandAttribute>() ??
                           throw new InvalidOperationException("No route selected")).Name,
                method => (bot, callbackQuery, cancellationToken) =>
                {
                    try
                    {
                        if (method.Invoke(this, [bot, callbackQuery, cancellationToken]) is Task task)
                            return task;
                        throw new InvalidOperationException($"Method {method.Name} not found");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error invoking method {method.Name}: {e.Message}");
                        throw;
                    }

                });
    }
    public Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancelToken)
    {
        var callbackQuery = update.CallbackQuery;
        if (callbackQuery == null)
        {
            return Task.CompletedTask;
        }
        if (string.IsNullOrEmpty(callbackQuery.Data))
                return Task.CompletedTask;
        var command = callbackQuery.Data.Split('#')[0];
        return _messageRoutes.TryGetValue(command, out var handler) ? 
            handler(botClient, callbackQuery, cancelToken) : 
            Task.CompletedTask;
    }

    [Command("courses")]
    public async Task GetCourses(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        var inlineKeyboardButtons = (await _apiService.GetCourses())
            .Select(course => new InlineKeyboardButton
            {
                Text = course.Title,
                CallbackData = $"course#{course.CourseId}"
            })
            .ToArray();
        var inlineKeyboardMarkup = ReplyMarkupHelper.CreateInlineKeyboard(inlineKeyboardButtons)
            .CreateInlineKeyboardMarkup();
        await botClient.EditMessageText(
            chatId: callbackQuery.Message!.Chat.Id,
            messageId: callbackQuery.Message!.MessageId,
            replyMarkup: inlineKeyboardMarkup,
            text: $"Список курсов", 
            cancellationToken: cancelToken);
    }
    
    [Command("course")]
    public async Task GetCourse(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        var courseId = int.Parse(callbackQuery.Data!.Split('#')[1]);
        var inlineKeyboardButtons = (await _apiService.GetModules(courseId))
            .Select(module => new InlineKeyboardButton
            {
                Text = module.Title,
                CallbackData = $"module#{module.ModuleId}"
            })
            .ToArray();
        var inlineKeyboardMarkup = ReplyMarkupHelper.CreateInlineKeyboard(inlineKeyboardButtons)
            .AddInlineKeyboardRow(new InlineKeyboardButton
            {
                Text = "Назад",
                CallbackData = "courses"
            })
            .CreateInlineKeyboardMarkup();
        await botClient.EditMessageText(
            chatId: callbackQuery.Message!.Chat.Id,
            messageId: callbackQuery.Message!.MessageId,
            replyMarkup: inlineKeyboardMarkup,
            text: $"Выберите модуль", 
            cancellationToken: cancelToken);
    }
    
    [Command("module")]
    public async Task GetModule(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        var lessonId = int.Parse(callbackQuery.Data!.Split('#')[1]);
        var inlineKeyboardButtons = (await _apiService.GetLessons(lessonId))
            .Select(lesson => new InlineKeyboardButton
            {
                Text = lesson.Title,
                CallbackData = $"lesson#{lesson.LessonId}"
            })
            .ToArray();
        var inlineKeyboardMarkup = ReplyMarkupHelper.CreateInlineKeyboard(inlineKeyboardButtons)
            .AddInlineKeyboardRow(new InlineKeyboardButton
            {
                Text = "Назад",
                CallbackData = callbackQuery.Data
            })
            .CreateInlineKeyboardMarkup();
        await botClient.EditMessageText(
            chatId: callbackQuery.Message!.Chat.Id,
            messageId: callbackQuery.Message!.MessageId,
            replyMarkup: inlineKeyboardMarkup,
            text: $"Выберите урок", 
            cancellationToken: cancelToken);
    }
    
    [Command("lesson")]
    public async Task GetLesson(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        var courseId = int.Parse(callbackQuery.Data!.Split('#')[1]);
        var inlineKeyboardButtons = (await _apiService.GetModules(courseId))
            .Select(module => new InlineKeyboardButton
            {
                Text = module.Title,
                CallbackData = $"module#{module.ModuleId}"
            })
            .ToArray();
        var inlineKeyboardMarkup = ReplyMarkupHelper.CreateInlineKeyboard(inlineKeyboardButtons)
            .AddInlineKeyboardRow(new InlineKeyboardButton
            {
                Text = "Назад",
                CallbackData = "courses"
            })
            .CreateInlineKeyboardMarkup();
        await botClient.EditMessageText(
            chatId: callbackQuery.Message!.Chat.Id,
            messageId: callbackQuery.Message!.MessageId,
            replyMarkup: inlineKeyboardMarkup,
            text: $"Выберите модуль", 
            cancellationToken: cancelToken);
    }
}