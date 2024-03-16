using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
public class TelegramBotHelper
{
    private string token;
    static GeraldicSignList table = new GeraldicSignList();
    TelegramBotClient client;
    public TelegramBotHelper(string token)
    {
        this.token = token;
    }
    public void GetUpdates()
    {
        client = new TelegramBotClient(token);
        var me = client.GetMeAsync().Result;
        if (me != null && !string.IsNullOrEmpty(me.Username))
        {
            int offset = 0;
            while (true)
            {
                try
                {
                    var updates = client.GetUpdatesAsync(offset).Result;
                    if (updates != null && updates.Count() > 0)
                    {
                        foreach(var update in updates)
                        {
                            processUpdate(client, update);
                            offset = update.Id + 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(1000);
            }
        }
    }

    private async void processUpdate(ITelegramBotClient botClient, Update update)
    {
        switch (update.Type)
        {
            case Telegram.Bot.Types.Enums.UpdateType.Message:
                var text = update.Message.Text;
                switch (text)
                {
                    case "Загрузить CSV файл":
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Загрузить CSV файл", replyMarkup: GetButtons());
                        break;

                    case "Выборка":
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Выборка", replyMarkup: GetChooseFilterButtons());
                        break;

                    case "Сортировка":
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Сортировка", replyMarkup: GetButtons());
                        break;

                    case "Скачать обработанный файл":
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Скачать обработанный файл", replyMarkup: GetButtons());
                        break;

                    case "Загрузить JSON файл":
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Загрузить JSON файл", replyMarkup: GetButtons());
                        break;

                    case "Фильтрация по Type":
                        await FilteringData.FilterByOneCondition(botClient, update, "Type", table);
                        break;  

                    case "Фильтрация по RegistrationDate":
                        await FilteringData.FilterByOneCondition(botClient, update, "RegistrationDate", table);
                        break;

                    case "Фильтрация по CertificateHolderName и RegistrationDate":
                        await FilteringData.FilterByTwoConditions(botClient, update, "CertificateHolderName", "RegistrationDate", table);
                        break;

                    default:
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Такой кнопки нет", replyMarkup: GetButtons());
                        break;
                }
                break;

            default:
                Console.WriteLine(update.Type + " Not implemented!");
                break;

        }
    }
    private static IReplyMarkup? GetChooseFilterButtons()
    {
        return new ReplyKeyboardMarkup
        (
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton ("Фильтрация по Type") },
                new List<KeyboardButton> { new KeyboardButton ("Фильтрация по RegistrationDate") },
                new List<KeyboardButton> { new KeyboardButton ("Фильтрация по CertificateHolderName и RegistrationDate") }
            }
        );
    }
    private static IReplyMarkup? GetButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            { 
                new List<KeyboardButton> { new KeyboardButton("Загрузить CSV файл"), new KeyboardButton ("Выборка") },
                new List<KeyboardButton> { new KeyboardButton("Сортировка"), new KeyboardButton ("Скачать обработанный файл") },
                new List<KeyboardButton> { new KeyboardButton("Загрузить JSON файл") }
            }
        );
    }
}
