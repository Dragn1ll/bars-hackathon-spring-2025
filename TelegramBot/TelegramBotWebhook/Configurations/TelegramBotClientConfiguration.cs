using Telegram.Bot.Types.Enums;

namespace TelegramBotWebhook.Configurations;

public class TelegramBotClientConfiguration
{
    public required string Token { get; init; }
    public required UpdateType[] AllowedUpdates { get; init; }
    public required string Url { get; init; }
}