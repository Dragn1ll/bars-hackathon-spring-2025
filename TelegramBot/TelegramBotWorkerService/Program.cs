using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBotWorkerService;
using TelegramBotWorkerService.Abstractions;
using TelegramBotWorkerService.Services;
using TelegramBotWorkerService.UpdateHandlers;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(_ =>
{
    var token = builder.Configuration["TelegramBotClientOptions:Token"];
    if (string.IsNullOrEmpty(token))
    {
        throw new NullReferenceException("Please provide a valid token");
    }
    return new TelegramBotClient(token);
});

builder.Services.AddSingleton<ITelegramBotService, TelegramBotService>();
builder.Services.AddKeyedSingleton<ICustomUpdateHandler, MessageUpdateHandler>(UpdateType.Message);
builder.Services.AddHostedService<Worker>();


var host = builder.Build();
host.Run();