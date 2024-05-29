using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using InputFile = Telegram.Bot.Types.InputFile;
using Message = Telegram.Bot.Types.Message;
using ParseMode = Telegram.Bot.Types.Enums.ParseMode;
using Update = Telegram.Bot.Types.Update;
using UpdateType = Telegram.Bot.Types.Enums.UpdateType;

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
        if (update.Message is not null)
        {
            await HandleMessageAsync(botClient, update.Message, cancellationToken);
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

    private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        switch (message.Text)
        {
            case "/start":
                await SendMainMenuAsync(botClient, message.Chat.Id, cancellationToken);
                break;
            case "Урок 1":
                await HandleLesson1Async(botClient, message, cancellationToken);
                break;
            case "Урок 2":
                await HandleLesson2Async(botClient, message, cancellationToken);
                break;
            case "Урок 3":
                await HandleLesson3Async(botClient, message, cancellationToken);
                break;
            case "Урок 4":
                await HandleLesson4Async(botClient, message, cancellationToken);
                break;
            case "Урок 5":
                await HandleLesson5Async(botClient, message, cancellationToken);
                break;
            case "Урок 6":
                await HandleLesson6Async(botClient, message, cancellationToken);
                break;
            case "Урок 7":
                await HandleLesson7Async(botClient, message, cancellationToken);
                break;
            case "Урок 8":
                await HandleLesson8Async(botClient, message, cancellationToken);
                break;
            case "Урок 9":
                await HandleLesson9Async(botClient, message, cancellationToken);
                break;
            case "Урок 10":
                await HandleLesson10Async(botClient, message, cancellationToken);
                break;
            case "Пройти тест":
                break;
            default:
            {
                await botClient.SendTextMessageAsync(
                    parseMode: ParseMode.Html,
                    chatId: message.Chat.Id,
                    text:
                    "Извините, я не понимаю этой команды. Введите \"Урок N\", где N - это номер урока от 1 до 10.\n" +
                    "\n<b>С более подробными аннтоациями можете в панели ниже</b>",
                    cancellationToken: cancellationToken);
                break;
            }
        }
    }

    

    private async Task SendMainMenuAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        // Создание кнопок главного меню
        var keyboard = new ReplyKeyboardMarkup(
            new[]
            {
                new[]
                {
                    new KeyboardButton("Урок 1")
                },
                new[]
                {
                    new KeyboardButton("Урок 2")
                },
                new[]
                {
                    new KeyboardButton("Урок 3")
                }
                ,
                new[]
                {
                    new KeyboardButton("Урок 4")
                },
                new[]
                {
                    new KeyboardButton("Урок 5")
                },
                new[]
                {
                    new KeyboardButton("Урок 6")
                },
                new[]
                {
                    new KeyboardButton("Урок 7")
                },
                new[]
                {
                    new KeyboardButton("Урок 8")
                }
            }
            
        );

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Добро пожаловать в бот для изучения Vim! Выберите урок:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
    


    private async Task HandleLesson1Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        string lessonText_1 = "Vim \\- довольно странный редактор\\. \n\n" +
                      "В нем есть режимы :/\n\n" +
                      "Режим для навигации/редактирования, режим для вставки текста, режим для выделения текста\\. \n\n" +
                      "Это дает Vim возможность по\\-настоящему использовать преимущества вашей клавиатуры, потому что он может сосредоточиться только на одной задаче за раз\\.";

        string lessonText_2 =
            "Давайте начнем отработку основ передвижения в normal режиме: `hjkl` позволяет перемещать курсор на один пробел в любом направлении\\.";
        string lessonText_3 = ">>      ↑\n" +
                              ">>← h j k l →\n" +
                              ">>        ↓";
        string lessonText_4 =
            "Теперь, когда мы освоили самые основные приемы, давайте посмотрим, как можно заставить редактор вести себя так, как вы привыкли: в режиме **Insert** Vim вставляет фрагменты текста и кода, как обычный редактор\\.\n\n" +
            "Введите i, чтобы перейти в режим **Insert**\\.\n" +
        "Наберите **<ESC>**, **<CTRL-[>** или **<CTRL-C>**, чтобы вернуться в **обычный режим**\\.";
        
        await botClient.SendPhotoAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            photo: InputFile.FromUri("https://postimg.cc/sBJhSkmf"),
            caption: lessonText_1,
            cancellationToken: cancellationToken);
        
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lessonText_2,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lessonText_3,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lessonText_4,
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
    private async Task HandleLesson4Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            
            text: "Это Урок 4. ",
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson5Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            
            text: "Это Урок 5. ",
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson6Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            
            text: "Это Урок 6. ",
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson7Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            
            text: "Это Урок 7. ",
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson8Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            
            text: "Это Урок 8. ",
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson9Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            
            text: "Это Урок 9. ",
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson10Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            
            text: "Это Урок 10. ",
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