using Telegram.Bot;
using Telegram.Bot.Types;
internal class Program
{
    static void Main(string[] args)
    {
        var client = new TelegramBotClient("6757896301:AAGpTPCVQBwt0dkJN5wYG4MD3dCmmUJWGyg");
        client.StartReceiving(Update, Error);
        Console.ReadLine();
    }

    async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;
        if (message.Text == null)
        {
            if (message.Text.ToLower().Contains("здорова"))
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Здорова пользователь");
                return;
            }
        }
    }

    async static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}

