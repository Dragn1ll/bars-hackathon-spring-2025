using TelegramBotWorkerService.Abstractions;

namespace TelegramBotWorkerService;

public class Worker(ITelegramBotService telegramBotService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await telegramBotService.StartAsync(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            //await telegramBotService.SendMessageAsync("привет", stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}