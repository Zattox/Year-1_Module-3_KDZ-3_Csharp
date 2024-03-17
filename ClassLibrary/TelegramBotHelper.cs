using System.Reflection.PortableExecutable;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
public class TelegramBotHelper
{
    private string token;
    private string pathFile;
    private static List<GeraldicSign> table;
    TelegramBotClient client;
    public TelegramBotHelper(string token)
    {
        this.token = token;
    }

    private static IReplyMarkup? GetInputButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton("Загрузить CSV файл"), new KeyboardButton("Загрузить JSON файл") }
            }
        );
    }
    private static IReplyMarkup? GetButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton("Загрузить CSV файл"), new KeyboardButton("Загрузить JSON файл")  },
                new List<KeyboardButton> { new KeyboardButton("Фильтрация по Type"), new KeyboardButton("Фильтрация по RegistrationDate") },
                new List<KeyboardButton> { new KeyboardButton("Фильтрация по CertificateHolderName и RegistrationDate") },
                new List<KeyboardButton> { new KeyboardButton("Сортировка по возрастанию"), new KeyboardButton ("Сортировка по убыванию") },
                new List<KeyboardButton> { new KeyboardButton("Скачать обработанный файл в JSON"), new KeyboardButton("Скачать обработанный файл в СSV") }
            }
        );
    }
    private async void ProcessUpdateAsync(ITelegramBotClient botClient, Update update)
    {
        switch (update.Type)
        {
            case Telegram.Bot.Types.Enums.UpdateType.Message:
                var text = update.Message.Text;
                if (table is null)
                {
                    if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                    {
                        pathFile = await CSVProcessing.DownloadCsvFile(botClient, update);
                        table = CSVProcessing.Read(pathFile, out List<int> bugs);
                        await client.SendTextMessageAsync(update.Message.Chat.Id, $"Обнаружены ошибки в {bugs.Count} строках, они были пропущены при записи", replyMarkup: GetButtons());
                        CSVProcessing.Write(botClient, update, table, pathFile);
                    } else
                    {
                        await client.SendTextMessageAsync(update.Message.Chat.Id, "Для других функции для начала загрузите данные из файла!", replyMarkup: GetInputButtons());
                    }
                    /* case "Загрузить JSON файл":
                         await client.SendTextMessageAsync(update.Message.Chat.Id, "Загрузить JSON файл", replyMarkup: GetInputButtons());
                         break;

                     default:
                         await client.SendTextMessageAsync(update.Message.Chat.Id, "Для других функции для начала загрузите данные из файла!", replyMarkup: GetInputButtons());
                         break;
                     */
                }
                else
                {
                    switch (text)
                    {
                        case "Фильтрация по Type":
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Фильтрация по Type", replyMarkup: GetButtons());
                            await FilteringData.FilterByOneConditionAsync(botClient, update, "Type", table);
                            break;

                        case "Фильтрация по RegistrationDate":
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Фильтрация по RegistrationDate", replyMarkup: GetButtons());
                            await FilteringData.FilterByOneConditionAsync(botClient, update, "RegistrationDate", table);
                            break;

                        case "Фильтрация по CertificateHolderName и RegistrationDate":
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Фильтрация по CertificateHolderName и RegistrationDate", replyMarkup: GetButtons());
                            await FilteringData.FilterByTwoConditionsAsync(botClient, update, "CertificateHolderName", "RegistrationDate", table);
                            break;

                        case "Сортировка по возрастанию":
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Сортировка по возрастанию", replyMarkup: GetButtons());
                            break;

                        case "Сортировка по убыванию":
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Сортировка по убыванию", replyMarkup: GetButtons());
                            break;

                        case "Скачать обработанный файл в JSON":
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Скачать обработанный файл в JSON", replyMarkup: GetButtons());
                            break;

                        case "Скачать обработанный файл в СSV":
                            await CSVProcessing.UploadCsvFile(botClient, update, pathFile);
                            break;

                        default:
                            await client.SendTextMessageAsync(update.Message.Chat.Id, "Такой функции нет", replyMarkup: GetButtons());
                            break;
                    }
                }
                break;

            default:
                Console.WriteLine(update.Type + " Not implemented!");
                break;

        }
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
                        foreach (var update in updates)
                        {
                            ProcessUpdateAsync(client, update);
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
}
