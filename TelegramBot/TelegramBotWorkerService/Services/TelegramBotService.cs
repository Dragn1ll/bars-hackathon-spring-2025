using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotWorkerService.Abstractions;

namespace TelegramBotWorkerService.Services;

public class TelegramBotService: ITelegramBotService
{
    private readonly ITelegramBotClient _client;
    private readonly Dictionary<UpdateType, ICustomUpdateHandler> _handlers;
    private static readonly UpdateType[] UpdateTypes = 
    [
        UpdateType.Message,
        //UpdateType.CallbackQuery
    ];
    public TelegramBotService(ITelegramBotClient client, IServiceProvider keyedServiceProvider)
    {
        _client = client;
        _handlers = UpdateTypes
            .ToDictionary<UpdateType, UpdateType, ICustomUpdateHandler>(type => type,
                type => keyedServiceProvider.GetRequiredKeyedService<ICustomUpdateHandler>(type));
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = UpdateTypes,
            DropPendingUpdates = true
        };
        _client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
        return Task.CompletedTask;
    }

    public async Task SendMessageAsync(string text, CancellationToken cancellationToken)
    {
        await _client.SendMessage(1840413780, text, cancellationToken: cancellationToken); // тут айди моего с ним чата
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await _handlers[update.Type].HandleUpdate(botClient, update, cancellationToken);
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.Message);
        return Task.CompletedTask;
    }
}