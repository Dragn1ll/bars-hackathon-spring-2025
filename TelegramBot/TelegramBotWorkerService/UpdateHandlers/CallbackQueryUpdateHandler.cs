using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
        var courseId = Guid.Parse(callbackQuery.Data!.Split('#')[1]);
        var course = await _apiService.GetCourse(courseId);
        if (course == null)
            return;
        var inlineKeyboardButtons = course.Modules
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
            text: $"*{course.Title}*\n_{course.Description}_",
            parseMode: ParseMode.Markdown,
            cancellationToken: cancelToken);
    }
    
    [Command("module")]
    public async Task GetModule(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        var moduleId = Guid.Parse(callbackQuery.Data!.Split('#')[1]);
        var module = await _apiService.GetModule(moduleId);
        if (module == null)
            return;
        var inlineKeyboardButtons = module.Lessons
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
                CallbackData = $"course#{module.LessonId}"
            })
            .CreateInlineKeyboardMarkup();
        try
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                replyMarkup: inlineKeyboardMarkup,
                text: $"*{module.Title}*",
                parseMode: ParseMode.Markdown,
                cancellationToken: cancelToken);
        }
        catch (Exception)
        {
            await botClient.DeleteMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId, 
                cancellationToken: cancelToken);
            await botClient.SendMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                replyMarkup: inlineKeyboardMarkup,
                text: $"*{module.Title}*",
                parseMode: ParseMode.Markdown,
                cancellationToken: cancelToken);
        }
    }
    
    [Command("lesson")]
    public async Task GetLesson(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        var lessonId = Guid.Parse(callbackQuery.Data!.Split('#')[1]);
        var lesson = await _apiService.GetLesson(lessonId);
        if (lesson == null)
            return;
        var inlineKeyboardMarkup = ReplyMarkupHelper.CreateInlineKeyboard(new InlineKeyboardButton
            {
                Text = "Назад",
                CallbackData = $"module#{lesson.ModuleId}"
            })
            .CreateInlineKeyboardMarkup();
        await botClient.EditMessageMedia(
            chatId: callbackQuery.Message!.Chat.Id,
            messageId: callbackQuery.Message!.MessageId,
            replyMarkup: inlineKeyboardMarkup,
            media:  new InputMediaDocument(InputFile.FromUri("https://avatars.mds.yandex.net/i?id=85bf7002b221dd0843622a3df5b9e273_l-10471914-images-thumbs&n=13"))
            {
                Caption = $"*_{lesson.Title}_*",
                ParseMode = ParseMode.MarkdownV2
            },
            cancellationToken: cancelToken);
    }
}