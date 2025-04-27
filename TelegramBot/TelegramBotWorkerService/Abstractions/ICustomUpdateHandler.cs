using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotWorkerService.Abstractions;

public interface ICustomUpdateHandler
{
    public Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancelToken);
}