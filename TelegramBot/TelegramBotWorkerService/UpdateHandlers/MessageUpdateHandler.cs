using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotWorkerService.Abstractions;
using TelegramBotWorkerService.Models;

namespace TelegramBotWorkerService.UpdateHandlers;

public class MessageUpdateHandler : ICustomUpdateHandler
{
    private readonly ITelegramApiService _apiService;
    private readonly Dictionary<string, Func<ITelegramBotClient, Message, CancellationToken, Task>> _messageRoutes;
    private readonly Func<ITelegramBotClient, Contact, CancellationToken, Task>? _contactHandler;
    public MessageUpdateHandler(ITelegramApiService apiService)
    {
        _apiService = apiService;
        _messageRoutes = GetType()
            .GetMethods()
            .Where(m => m.IsDefined(typeof(CommandAttribute), false))
            .ToDictionary<MethodInfo, string, Func<ITelegramBotClient, Message, CancellationToken, Task>>(
                method => (method.GetCustomAttribute<CommandAttribute>() ??
                           throw new InvalidOperationException("No route selected")).Name,
                method => (bot, message, cancellationToken) =>
                {
                    try
                    {
                        if (method.Invoke(this, [bot, message, cancellationToken]) is Task task)
                            return task;
                        throw new InvalidOperationException($"Method {method.Name} not found");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error invoking method {method.Name}: {e.Message}");
                        throw;
                    }

                });
        _contactHandler = typeof(MessageUpdateHandler)
            .GetMethods()
            .Where(m => m.IsDefined(typeof(ContactHandlerAttribute), false))
            .Select(method => (Func<ITelegramBotClient, Contact, CancellationToken, Task>)((bot, contact, cancellationToken) =>
            {
                if (method.Invoke(this, [bot, contact, cancellationToken]) is Task task)
                    return task;
                throw new InvalidOperationException($"Method {method.Name} did not return Task");
            }))
            .FirstOrDefault();
    }

    public Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancelToken)
    {
        var message = update.Message;
        if (message == null)
            throw new InvalidOperationException("Update message is null");
        if (message.Contact is { } contact && _contactHandler != null)
        {
            _contactHandler(botClient, contact, cancelToken);
            return Task.CompletedTask;
        }
        if (message.Text == null)
            throw new NullReferenceException("Text is null");
        return _messageRoutes.TryGetValue(message.Text, out var handler) ? 
            handler(botClient, message, cancelToken) : 
            HandleUnsupportedCommand(botClient, message, cancelToken);
    }
    [Command("/start")]
    public async Task Start(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
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

        await botClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Здравствуйте, вас приветствует бот для помощи с курсом. Для начала пройдите регистрацию",
            replyMarkup: replyMarkup, cancellationToken: cancellationToken);
    }

    [ContactHandler]
    public async Task Register(ITelegramBotClient botClient, Contact contact, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Номер: {contact.PhoneNumber}");
        await botClient.SendMessage(contact.UserId!, 
            "Вы успешно зарегестрировались!",
            replyMarkup: ReplyMarkupHelper.CreateKeyboard("Курсы")
                .CreateKeyboardMarkup(new ReplyKeyboardOptions
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = false
                }),
            cancellationToken: cancellationToken);
    }

    [Command("Курсы")]
    public async Task GetCourses(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
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
        await botClient.SendMessage(message.Chat.Id, 
            "Список курсов",
            replyMarkup: inlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    public async Task HandleUnsupportedCommand(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken)
    {
        await botClient.SendMessage(message.Chat.Id, 
            "Неизвестная комманда",
            cancellationToken: cancellationToken);
    }
}