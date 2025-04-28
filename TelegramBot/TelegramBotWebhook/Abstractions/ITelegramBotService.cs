using Telegram.Bot.Types;

namespace TelegramBotWebhook.Abstractions;

public interface ITelegramBotService
{
    public Task SetWebHookAsync(CancellationToken cancellationToken = default);

    public Task HandleUpdateAsync(Update update,
        CancellationToken cancellationToken);

    public Task HandleErrorAsync(Exception exception,
        CancellationToken cancellationToken);
}