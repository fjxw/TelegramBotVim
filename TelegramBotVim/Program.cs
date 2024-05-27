using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotVim;

public class TelegramBot
{
    private readonly TelegramBotClient _botClient;

    public TelegramBot(string botToken)
    {
        _botClient = new TelegramBotClient(botToken);
    }

    public async Task StartAsync()
    {
        var cts = new CancellationTokenSource();

        // Настройка обработчиков событий
        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // получать все типы обновлений
            },
            cancellationToken: cts.Token
        );

        Console.WriteLine("Бот успешно запущен");
        await Task.Delay(Timeout.Infinite, cts.Token);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Обработка входящих сообщений
        if (update.Message is not null)
        {
            await HandleMessageAsync(botClient, update.Message, cancellationToken);
        }
        // Обработка нажатий на кнопки
        else if (update.CallbackQuery is not null)
        {
            await HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
        }
    }
    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Обработка ошибок при получении обновлений
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message != null)
        {
            // Обработка сообщений
            if (update.Message.Text == "/start")
            {
                await SendMainMenuAsync(botClient, update.Message.Chat.Id, cancellationToken);
            }
            else if (update.Message.Text.Length == 1 && int.TryParse(update.Message.Text, out int lessonNumber))
            {
                await HandleLessonAsync(botClient, update.Message, lessonNumber, cancellationToken);
            }
            else
            {
                // Обработка других сообщений
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Извините, я не понимаю этой команды. Введите номер урока от 1 до 10.",
                    cancellationToken: cancellationToken);
            }
        }
        else if (update.CallbackQuery != null)
        {
            // Обработка callback_data
            await HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
        }
    }


    // Обработка нажатия на кнопку с использованием callback_data
    private async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        switch (callbackQuery.Data)
        {
            case "lesson1":
                await HandleLesson1Async(botClient, callbackQuery.Message, cancellationToken);
                break;
            case "lesson2":
                await HandleLesson2Async(botClient, callbackQuery.Message, cancellationToken);
                break;
            case "lesson3":
                await HandleLesson3Async(botClient, callbackQuery.Message, cancellationToken);
                break;
            
        }
    }
    private async Task HandleLessonAsync(ITelegramBotClient botClient, Message message, int lessonNumber, CancellationToken cancellationToken)
    {
        switch (lessonNumber)
        {
            case 1:
                await HandleLesson1Async(botClient, message, cancellationToken);
                break;
            case 2:
                await HandleLesson2Async(botClient, message, cancellationToken);
                break;
            case 3:
                await HandleLesson3Async(botClient, message, cancellationToken);
                break;
            // Добавьте обработку других уроков
            default:
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Извините, урока {lessonNumber} пока нет.",
                    cancellationToken: cancellationToken);
                break;
        }
    }


    private async Task SendMainMenuAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        // Создание кнопок главного меню
        var inlineKeyboard = new InlineKeyboardMarkup(
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Урок 1 - что такое Vim?", "lesson1"),
                    InlineKeyboardButton.WithCallbackData("Урок 2", "lesson2"),
                    InlineKeyboardButton.WithCallbackData("Урок 3", "lesson3")
                }
            }
        );

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Добро пожаловать в бот для изучения Vim! Выберите урок:",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task HandleLesson1Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Это Урок 1. ",
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson2Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Это Урок 2. ",
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson3Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            
            text: "Это Урок 3. ",
            cancellationToken: cancellationToken);
    }
}

public static class Program
{
    public static async Task Main(string[] args)
    {
        var bot = new TelegramBot("7217044323:AAGsftZoscCzEGGOv6rsuB0VRvstL6iVIyc");
        await bot.StartAsync();
    }
}