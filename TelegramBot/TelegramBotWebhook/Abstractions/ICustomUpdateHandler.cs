using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotWebhook.Abstractions;

public interface ICustomUpdateHandler
{
    public Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancelToken);
}