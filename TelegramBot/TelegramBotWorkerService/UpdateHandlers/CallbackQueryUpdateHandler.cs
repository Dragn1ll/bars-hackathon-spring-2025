using System.Reflection;
using Domain.Models.Dto.General;
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
        try
        {
            var courses = await _apiService.GetCourses(callbackQuery.From.Id);
            if (courses == null)
                return;
            var inlineKeyboardButtons = courses
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
        catch (HttpRequestException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Ошибка на стороне сервера, пожалуйста, запросите курсы еще раз",
                cancellationToken: cancelToken);
        }
        catch (KeyNotFoundException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Извините, курсов не найдено",
                cancellationToken: cancelToken);
        }
        catch (UnauthorizedAccessException)
        {
            var requestContactButton = KeyboardButton.WithRequestContact("Зарегистрироваться");
            var replyMarkup = ReplyMarkupHelper.CreateKeyboard(requestContactButton)
                .CreateKeyboardMarkup(new ReplyKeyboardOptions
                {
                    ResizeKeyboard = true,
                    IsPersistent = true,
                    InputFieldPlaceholder = "Зарегистрируйтесь",
                    OneTimeKeyboard = true
                });
            await botClient.DeleteMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId, 
                cancellationToken: cancelToken);
            await botClient.SendMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                text: "Вас нет в системе, пожалуйста, попробуйте зарегистрироваться",
                replyMarkup: replyMarkup,
                cancellationToken: cancelToken);
        }
        
    }
    
    [Command("course")]
    public async Task GetCourse(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        try
        {
            var courseId = Guid.Parse(callbackQuery.Data!.Split('#')[1]);
            var course = await _apiService.GetCourse(callbackQuery.From.Id, courseId);
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
        catch (HttpRequestException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Ошибка на стороне сервера, пожалуйста, запросите курсы еще раз",
                cancellationToken: cancelToken);
        }
        catch (KeyNotFoundException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Извините, такого курса не найдено",
                cancellationToken: cancelToken);
        }
        catch (UnauthorizedAccessException)
        {
            var requestContactButton = KeyboardButton.WithRequestContact("Зарегистрироваться");
            var replyMarkup = ReplyMarkupHelper.CreateKeyboard(requestContactButton)
                .CreateKeyboardMarkup(new ReplyKeyboardOptions
                {
                    ResizeKeyboard = true,
                    IsPersistent = true,
                    InputFieldPlaceholder = "Зарегистрируйтесь",
                    OneTimeKeyboard = true
                });
            await botClient.DeleteMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                cancellationToken: cancelToken);
            await botClient.SendMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                text: "Вас нет в системе, пожалуйста, попробуйте зарегистрироваться",
                replyMarkup: replyMarkup,
                cancellationToken: cancelToken);
        }
    }
    
    [Command("module")]
    public async Task GetModule(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        ModuleDto? module;
        var moduleId = Guid.Parse(callbackQuery.Data!.Split('#')[1]);
        try
        {
            module = await _apiService.GetModule(callbackQuery.From.Id, moduleId);
            if (module == null)
                return;
        }
        catch (HttpRequestException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Ошибка на стороне сервера, пожалуйста, запросите курсы еще раз",
                cancellationToken: cancelToken);
            return;
        }
        catch (KeyNotFoundException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Извините, модуль не найден",
                cancellationToken: cancelToken);
            return;
        }
        catch (UnauthorizedAccessException)
        {
            var requestContactButton = KeyboardButton.WithRequestContact("Зарегистрироваться");
            var replyMarkup = ReplyMarkupHelper.CreateKeyboard(requestContactButton)
                .CreateKeyboardMarkup(new ReplyKeyboardOptions
                {
                    ResizeKeyboard = true,
                    IsPersistent = true,
                    InputFieldPlaceholder = "Зарегистрируйтесь",
                    OneTimeKeyboard = true
                });
            await botClient.DeleteMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId, 
                cancellationToken: cancelToken);
            await botClient.SendMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                text: "Вас нет в системе, пожалуйста, попробуйте зарегистрироваться",
                replyMarkup: replyMarkup,
                cancellationToken: cancelToken);
            return;
        }
            
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
                CallbackData = $"course#{module.CourseId}"
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
        try
        {
            var lessonId = Guid.Parse(callbackQuery.Data!.Split('#')[1]);
            var lesson = await _apiService.GetLesson(callbackQuery.From.Id, lessonId);
            var fileUrls = await _apiService.GetLessonContent(callbackQuery.From.Id, lessonId);
            if (lesson == null || fileUrls == null)
                return;
            await using var longReadFileStream = File.OpenRead("D:\\2ndCourse\\bars-hackathon-spring-2025\\TelegramBot\\TelegramBotWorkerService\\Files\\1 Лонгрид. тайм-менеджмент тим лида.docx");
            await using var pdfFileStream = File.OpenRead("D:\\2ndCourse\\bars-hackathon-spring-2025\\TelegramBot\\TelegramBotWorkerService\\Files\\Книги\\Сверх продуктивность, Михаил Алистер.pdf");
            InputMediaDocument[] inputDocuments = [new(longReadFileStream), new(pdfFileStream)];
            var inlineKeyboardMarkup = ReplyMarkupHelper.CreateInlineKeyboard(new InlineKeyboardButton
                {
                    Text = "Назад",
                    CallbackData = $"module#{lesson.ModuleId}"
                },
                new InlineKeyboardButton
                {
                    Text = "Начать тест",
                    CallbackData = $"start-test#{lesson.LessonId}"
                })
                .CreateInlineKeyboardMarkup();
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                parseMode: ParseMode.Markdown,
                replyMarkup: inlineKeyboardMarkup,
                text: $"*{lesson.Title}*",
                cancellationToken: cancelToken);
            await botClient.SendMediaGroup(
                chatId: callbackQuery.Message!.Chat.Id,
                media: inputDocuments,
                cancellationToken: cancelToken);
        }
        catch (HttpRequestException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Ошибка на стороне сервера, пожалуйста, запросите курсы еще раз",
                cancellationToken: cancelToken);
        }
        catch (KeyNotFoundException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Извините, урок не найден",
                cancellationToken: cancelToken);
        }
        catch (UnauthorizedAccessException)
        {
            var requestContactButton = KeyboardButton.WithRequestContact("Зарегистрироваться");
            var replyMarkup = ReplyMarkupHelper.CreateKeyboard(requestContactButton)
                .CreateKeyboardMarkup(new ReplyKeyboardOptions
                {
                    ResizeKeyboard = true,
                    IsPersistent = true,
                    InputFieldPlaceholder = "Зарегистрируйтесь",
                    OneTimeKeyboard = true
                });
            await botClient.DeleteMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId, 
                cancellationToken: cancelToken);
            await botClient.SendMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                text: "Вас нет в системе, пожалуйста, попробуйте зарегистрироваться",
                replyMarkup: replyMarkup,
                cancellationToken: cancelToken);
        }
    }

    [Command("start-test")]
    public async Task StartTest(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        try
        {
            var lessonId = Guid.Parse(callbackQuery.Data!.Split('#')[1]);
            var question = await _apiService.StartTest(callbackQuery.From.Id, lessonId);
            if (question == null)
                return;
            var inlineKeyboardButtons = question.Answers
                .Select(answer => new InlineKeyboardButton
                {
                    Text = answer.Text,
                    CallbackData = $"next#{answer.AnswerId}"
                })
                .ToArray();
            var inlineKeyboardMarkup = ReplyMarkupHelper.CreateInlineKeyboard(inlineKeyboardButtons)
                .CreateInlineKeyboardMarkup();
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                parseMode: ParseMode.Markdown,
                replyMarkup: inlineKeyboardMarkup,
                text: $"*{question.QuestionText}*",
                cancellationToken: cancelToken);
        }
        catch (HttpRequestException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Ошибка на стороне сервера, пожалуйста, запросите курсы еще раз",
                cancellationToken: cancelToken);
        }
        catch (KeyNotFoundException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Вы прошли тест",
                cancellationToken: cancelToken);
        }
        catch (UnauthorizedAccessException)
        {
            var requestContactButton = KeyboardButton.WithRequestContact("Зарегистрироваться");
            var replyMarkup = ReplyMarkupHelper.CreateKeyboard(requestContactButton)
                .CreateKeyboardMarkup(new ReplyKeyboardOptions
                {
                    ResizeKeyboard = true,
                    IsPersistent = true,
                    InputFieldPlaceholder = "Зарегистрируйтесь",
                    OneTimeKeyboard = true
                });
            await botClient.DeleteMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId, 
                cancellationToken: cancelToken);
            await botClient.SendMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                text: "Вас нет в системе, пожалуйста, попробуйте зарегистрироваться",
                replyMarkup: replyMarkup,
                cancellationToken: cancelToken);
        }
    }
    
    [Command("next")]
    public async Task AnswerNextQuestion(ITelegramBotClient botClient, CallbackQuery callbackQuery,
        CancellationToken cancelToken)
    {
        try
        {
            var answerId = Guid.Parse(callbackQuery.Data!.Split('#')[1]);
            var newQuestion = await _apiService.SendAnswer(callbackQuery.From.Id, answerId);
            if (newQuestion == null)
                return;
            var inlineKeyboardButtons = newQuestion.Answers
                .Select(answer => new InlineKeyboardButton
                {
                    Text = answer.Text,
                    CallbackData = $"next#{answer.AnswerId}"
                })
                .ToArray();
            var inlineKeyboardMarkup = ReplyMarkupHelper.CreateInlineKeyboard(inlineKeyboardButtons)
                .CreateInlineKeyboardMarkup();
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                replyMarkup: inlineKeyboardMarkup,
                text: $"*{newQuestion.QuestionText}*",
                cancellationToken: cancelToken);
        }
        catch (HttpRequestException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Ошибка на стороне сервера, пожалуйста, запросите курсы еще раз",
                cancellationToken: cancelToken);
        }
        catch (KeyNotFoundException)
        {
            await botClient.EditMessageText(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId,
                text: "Вы прошли тест",
                cancellationToken: cancelToken);
        }
        catch (UnauthorizedAccessException)
        {
            var requestContactButton = KeyboardButton.WithRequestContact("Зарегистрироваться");
            var replyMarkup = ReplyMarkupHelper.CreateKeyboard(requestContactButton)
                .CreateKeyboardMarkup(new ReplyKeyboardOptions
                {
                    ResizeKeyboard = true,
                    IsPersistent = true,
                    InputFieldPlaceholder = "Зарегистрируйтесь",
                    OneTimeKeyboard = true
                });
            await botClient.DeleteMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message!.MessageId, 
                cancellationToken: cancelToken);
            await botClient.SendMessage(
                chatId: callbackQuery.Message!.Chat.Id,
                text: "Вас нет в системе, пожалуйста, попробуйте зарегистрироваться",
                replyMarkup: replyMarkup,
                cancellationToken: cancelToken);
        }
    }
}