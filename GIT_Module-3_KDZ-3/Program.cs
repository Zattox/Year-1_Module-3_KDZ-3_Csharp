using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
internal class Program
{
    private static string token { get; set; } = "6757896301:AAGpTPCVQBwt0dkJN5wYG4MD3dCmmUJWGyg";
    private static TelegramBotClient client;
    static void Main(string[] args)
    {
        client = new TelegramBotClient(token);
        client.StartReceiving(Update, Error);
        Console.ReadLine();
    }

    async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;
        if (message.Text != null)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Здорова пользователь", replyMarkup: GetButtons());
            return;
        }
    }

    private static IReplyMarkup? GetButtons()
    {
        return new ReplyKeyboardMarkup
        (   
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton>{ new KeyboardButton("Загрузить CSV файл"), new KeyboardButton ("Выборка") },
                new List<KeyboardButton>{ new KeyboardButton("Сортировка"), new KeyboardButton ("Скачать обработанный файл")},
                new List<KeyboardButton>{ new KeyboardButton("Загрузить JSON файл")}
            }
        );
    }

    async static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}

