using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotWebhook.Abstractions;
using TelegramBotWebhook.Configurations;
using TelegramBotWebhook.Services;
using TelegramBotWorkerService.UpdateHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TelegramBotClientConfiguration>(builder.Configuration.GetSection("TelegramBotClientOptions"));

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
builder.Services.AddSingleton<ITelegramApiService, FakeTelegramApiService>();

var app = builder.Build();


app.MapGet("/custom", () => Results.Ok("Welcome to Telegram Bot"));

app.MapGet("/set-webhook", async (ITelegramBotService botService, CancellationToken cancellationToken) =>
{
    await botService.SetWebHookAsync(cancellationToken);
    return Results.Ok("webhook set");
});
app.MapPost("/bot", async ([FromServices] ITelegramBotService botService, [FromBody] Update update, CancellationToken cancellationToken) =>
{
    try
    {
        await botService.HandleUpdateAsync(update, cancellationToken);
    }
    catch (Exception ex)
    {
        await botService.HandleErrorAsync(ex, cancellationToken);
    }
});

app.Run();