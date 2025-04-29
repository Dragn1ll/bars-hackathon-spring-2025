using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBotWorkerService;
using TelegramBotWorkerService.Abstractions;
using TelegramBotWorkerService.Services;
using TelegramBotWorkerService.UpdateHandlers;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddEnvironmentVariables();
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
builder.Services.AddKeyedSingleton<ICustomUpdateHandler, CallbackQueryUpdateHandler>(UpdateType.CallbackQuery);
builder.Services.AddSingleton<ITelegramApiService, TelegramApiService>(_ =>
{
    var client = new HttpClient();
    client.BaseAddress = new Uri(builder.Configuration["TelegramApiService:BaseAddress"] ?? 
                                 throw new NullReferenceException("Please provide a valid TelegramApiService"));
    var telegramApiService = new TelegramApiService(client);
    return telegramApiService;
});
builder.Services.AddHostedService<Worker>();


var host = builder.Build();
host.Run();