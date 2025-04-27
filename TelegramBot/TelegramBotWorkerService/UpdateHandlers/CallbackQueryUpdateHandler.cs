using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotWorkerService.Abstractions;

namespace TelegramBotWorkerService.UpdateHandlers;

public class CallbackQueryUpdateHandler: ICustomUpdateHandler
{
    public Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancelToken)
    {
        throw new NotImplementedException();
    }
}