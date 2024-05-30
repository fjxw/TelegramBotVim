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
        
        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() 
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
            case "Урок 1 - Первые шаги":
                await HandleLesson1Async(botClient, message, cancellationToken);
                break;
            case "Урок 2 - Быстрые передвижения":
                await HandleLesson2Async(botClient, message, cancellationToken);
                break;
            case "Урок 3 - Ввод текста":
                await HandleLesson3Async(botClient, message, cancellationToken);
                break;
            case "Урок 4 - Выделение текста":
                await HandleLesson4Async(botClient, message, cancellationToken);
                break;
            case "Урок 5 - Поиск":
                await HandleLesson5Async(botClient, message, cancellationToken);
                break;
            case "Урок 6 - Вставка и копирование":
                await HandleLesson6Async(botClient, message, cancellationToken);
                break;
            case "Урок 7 - Режим командной строки":
                await HandleLesson7Async(botClient, message, cancellationToken);
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
        var keyboard = new ReplyKeyboardMarkup(
            new[]
            {
                new[]
                {
                    new KeyboardButton("Урок 1 - Первые шаги")
                },
                new[]
                {
                    new KeyboardButton("Урок 2 - Быстрые передвижения")
                },
                new[]
                {
                    new KeyboardButton("Урок 3 - Ввод текста")
                }
                ,
                new[]
                {
                    new KeyboardButton("Урок 4 - Выделение текста")
                },
                new[]
                {
                    new KeyboardButton("Урок 5 - Поиск")
                },
                new[]
                {
                    new KeyboardButton("Урок 6 - Вставка и копирование")
                },
                new[]
                {
                    new KeyboardButton("Урок 7 - Режим командной строки")
                }
            }
            
        );
        await botClient.SendPhotoAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: chatId,
            photo: InputFile.FromUri("https://post-images.org/photo-page.php?photo=tX7lY4zx"),
            caption: "Добро пожаловать в бот для изучения Vim\\! Выберите урок:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson1Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        string lesson1Text_1 = "Vim \\- довольно странный редактор\\. \n\n" +
                      "В нем есть режимы :/\n\n" +
                      "Режим для навигации/редактирования, режим для вставки текста, режим для выделения текста\\. \n\n" +
                      "Это дает Vim возможность по\\-настоящему использовать преимущества вашей клавиатуры, потому что он может сосредоточиться только на одной задаче за раз\\.";
        string lesson1Text_2 =
            "Давайте начнем отработку основ передвижения в normal режиме: `hjkl` позволяет перемещать курсор на один пробел в любом направлении\\.";
        string lesson1Text_3 = ">>      ↑\n" +
                              ">>← h j k l →\n" +
                              ">>        ↓";
        string lesson1Text_4 =
            "Теперь, когда мы освоили самые основные приемы, давайте посмотрим, как можно заставить редактор вести себя так, как вы привыкли: в режиме **Insert** Vim вставляет фрагменты текста и кода, как обычный редактор\\.\n\n" +
            "\\- Введите i, чтобы перейти в режим `Insert`\\.\n" +
        "\\- Нажмите `ESC`, `CTRL` или `CTRL\\-C`, чтобы вернуться в `обычный режим`\\.";
        await botClient.SendPhotoAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            photo: InputFile.FromUri("https://post-images.org/photo-page.php?photo=m5sCSMn8"),
            caption: lesson1Text_1,
            cancellationToken: cancellationToken);
        
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson1Text_2,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson1Text_3,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson1Text_4,
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson2Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        string lesson2Text_1 =
            "Пришло время попрактиковаться в базовых движениях, чтобы перемещаться по редактору с огромной скоростью\\.\n";
        string lesson2Text_2 =
            "Давайте начнем с двух замечательных движений для перемещения по горизонтали:\n\n\\- `w` для перемещения слово за словом\n\\- `b` для перемещения назад слово за словом\\.";
        string lesson2Text_3 = "\nТеперь попробуем кое\\-что другое:\n\n\\- `e` для перехода к концу слова\n\\- `ge` для перехода к концу предыдущего слова\\.";
        string lesson2Text_4 = "\nОт `k` и `j`, мы переходим к более быстрому способу вертикального маневрирования с помощью `\\}`:\n\n\\- `\\}` перебрасывает целые абзацы вниз\n\\- `\\{` аналогично, но вверх\\.";
        string lesson2Text_5 = "\nСчетчики \\- это числа, которые позволяют умножить эффект от команды:\n\n\n\\{count\\}\\{command\\}\n\n\nПопробуйте сами\\! Введите `2w`, чтобы переместиться на два слова вперед\\.";
        await botClient.SendPhotoAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            photo: InputFile.FromUri("https://post-images.org/photo-page.php?photo=utL9G8KE"),
            caption: lesson2Text_1,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson2Text_2,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson2Text_3,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson2Text_4,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson2Text_5,
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson3Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        string lesson3Text_1 = "До сих пор мы много внимания уделяли режиму _Normal mode_ и совсем не обращали внимания на режим _Insert mode_\\. Давайте исправим это, потому что в режиме _Insert_ в Vim есть гораздо больше возможностей, чем вы можете себе представить\\.\n";
        string lesson3Text_2 = "\\- `i` позволяет вставить текст перед курсором\n\\- `a` позволяет вставить текст после курсора\n\\- `I` позволяет вставить текст в начало строки\n\\- `A` позволяет вставить текст в конец строки\\.\n";
        await botClient.SendPhotoAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            photo: InputFile.FromUri("https://post-images.org/photo-page.php?photo=w37teKa5"),
            caption: lesson3Text_1,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson3Text_2,
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson4Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        string lesson4Text_1 = "**Визуальный режим** \\- это эквивалент перемещения мыши\\. Давайте попробуем три различных способа выделения текста в визуальном режиме:\n";
        string lesson4Text_2 = "\\- `v` для визуального режима по символам\n\\- `V` для визуального режима по строкам\n\\- `C\\-V` для блочного визуального режима\\.";
        await botClient.SendPhotoAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            photo: InputFile.FromUri("https://post-images.org/photo-page.php?photo=emqv08Xb"),
            caption: lesson4Text_1,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson4Text_2,
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson5Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        string lesson5Text_1 = "Для поиска в Vim вы должны быть в обычном режиме\\. Когда вы запускаете редактор Vim, вы находитесь в этом режиме\\. Чтобы вернуться в нормальный режим из любого другого режима, просто нажмите клавишу Esc\\.\n";
        string lesson5Text_2 = "\nVim позволяет быстро находить текст с помощью `\\/` и `\\?`";
        string lesson5Text_3 =
            "\nВажно отметить, что команда поиска ищет шаблон как строку, а не целое слово\\. Если, например, вы искали «gnu», поиск совпадет с тем, что «gnu» встроено в слова большего размера, такие как «cygnus» или «magnum»\\.";
        string lesson5Text_4 = "\nДля поиска всего слова начните поиск, нажав \\/ или \\? , введите \\< чтобы отметить начало слова, введите шаблон поиска, введите \\> чтобы отметить конец слова, и нажмите **Enter** чтобы выполнить поиск\\.";
        await botClient.SendPhotoAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            photo: InputFile.FromUri("https://post-images.org/photo-page.php?photo=UIg4KLNW"),
            caption: lesson5Text_1,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson5Text_2,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson5Text_3,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson5Text_4,
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson6Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        string lesson6Text_1 = "В обычном режиме можно копировать с помощью `y\\{шаг\\}`\\.";
        string lesson6Text_2 = "\nВ обычном режиме можно использовать p для вставки после курсора или P для вставки перед курсором\\. ";
        await botClient.SendPhotoAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            photo: InputFile.FromUri("https://post-images.org/photo-page.php?photo=iDJpEfXl"),
            caption: lesson6Text_1,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson6Text_2,
            cancellationToken: cancellationToken);
    }
    private async Task HandleLesson7Async(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        string lesson7Text_1 = "Чтобы перейти в командный режим, находясь в обычном режиме — нажмите `\\:` \\(Shift\\)\\. В нижней строке состояния Vim должно появиться двоеточие\\. Это означает, что командный режим активирован и ожидает ввода команд\\.\n";
        string lesson7Text_2 = "Существует множество команд для работы с документом\\. Все команды начинаются с `\\:`\\. \n";
        string lesson7Text_3 = "\\- ZQ или q\\! — выйти без сохранения\n\\- qa\\! — выйти из всех файлов без сохранения\n\\- ZZ или wq или x — записать и выйти\n\\- w — записать файл\n\\- sav имя\\_файла — сохранить как\n\\- w\\! — сохранить в новый файл\n\\- sh — свернуть Vim и перейти в Shell";
        await botClient.SendPhotoAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            photo: InputFile.FromUri("https://post-images.org/photo-page.php?photo=2irJVjHg"),
            caption: lesson7Text_1,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson7Text_2,
            cancellationToken: cancellationToken);
        await botClient.SendTextMessageAsync(
            parseMode: ParseMode.MarkdownV2,
            chatId: message.Chat.Id,
            text: lesson7Text_3,
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