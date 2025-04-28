using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotWebhook.Abstractions;
using TelegramBotWebhook.Configurations;

namespace TelegramBotWebhook.Services;

public class TelegramBotService: ITelegramBotService
{
    private readonly ITelegramBotClient _client;
    private readonly Dictionary<UpdateType, ICustomUpdateHandler> _handlers;
    private readonly UpdateType[] _updateTypes;
    private readonly string _url;
    public TelegramBotService(ITelegramBotClient client, IServiceProvider keyedServiceProvider, IOptions<TelegramBotClientConfiguration> options)
    {
        _client = client;
        _updateTypes = options.Value.AllowedUpdates;
        _url = options.Value.Url;
        _handlers = _updateTypes
            .ToDictionary<UpdateType, UpdateType, ICustomUpdateHandler>(type => type,
                type => keyedServiceProvider.GetRequiredKeyedService<ICustomUpdateHandler>(type));
    }
    public async Task SetWebHookAsync(CancellationToken cancellationToken)
    {
        await _client.SetWebhook(
            url: $"{_url}bot",
            maxConnections: 40,
            allowedUpdates: _updateTypes,
            dropPendingUpdates: true,
            cancellationToken: cancellationToken);
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        await _handlers[update.Type].HandleUpdate(_client, update, cancellationToken);
    }

    public Task HandleErrorAsync(Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.Message);
        return Task.CompletedTask;
    }
}