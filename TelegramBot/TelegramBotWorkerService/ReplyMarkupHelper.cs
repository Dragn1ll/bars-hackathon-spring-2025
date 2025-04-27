using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotWorkerService.Models;

namespace TelegramBotWorkerService;

public static class ReplyMarkupHelper
{
    public static InlineKeyboardButton[][] CreateInlineKeyboard(params InlineKeyboardButton[] buttons)
    {
        return [buttons];
    }

    public static InlineKeyboardButton[][] AddInlineKeyboardRow(this InlineKeyboardButton[][] existingButtonRows,
        params InlineKeyboardButton[] buttonsToAdd)
    {
        return existingButtonRows.Concat([buttonsToAdd]).ToArray();
    }
    
    public static InlineKeyboardMarkup CreateInlineKeyboardMarkup(this InlineKeyboardButton[][] buttonRows)
    {
        return new InlineKeyboardMarkup(buttonRows);
    }

    public static KeyboardButton[][] CreateKeyboard(params KeyboardButton[] buttons)
    {
        return [buttons];
    }

    public static KeyboardButton[][] AddKeyboardRow(this KeyboardButton[][] existingButtonRows,
        params KeyboardButton[] buttonsToAdd)
    {
        return existingButtonRows.Concat([buttonsToAdd]).ToArray();
    }

    public static ReplyKeyboardMarkup CreateKeyboardMarkup(this KeyboardButton[][] buttonRows, ReplyKeyboardOptions? options = null)
    {
        var customOptions = options ?? ReplyKeyboardOptions.DefaultOptions;
        return new ReplyKeyboardMarkup(buttonRows)
        {
            ResizeKeyboard = customOptions.ResizeKeyboard,
            OneTimeKeyboard = customOptions.OneTimeKeyboard,
            IsPersistent = customOptions.IsPersistent,
            InputFieldPlaceholder = customOptions.InputFieldPlaceholder,
            Selective = customOptions.Selective
        };
    }
}