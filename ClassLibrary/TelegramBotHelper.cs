using Microsoft.VisualBasic;
using System.ComponentModel.Design;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
public class TelegramBotHelper
{
    public const int CountOfHeaders = 9;
    private string token;
    private static string pathFile;
    private static List<GeraldicSign> table;
    TelegramBotClient client;
    public TelegramBotHelper(string token)
    {
        this.token = token;
    }
    private static IReplyMarkup? GetButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton("Фильтрация по Type"), new KeyboardButton("Фильтрация по RegistrationDate") },
                new List<KeyboardButton> { new KeyboardButton("Фильтрация по CertificateHolderName и RegistrationDate") },
                new List<KeyboardButton> { new KeyboardButton("Сортировка по возрастанию"), new KeyboardButton ("Сортировка по убыванию") },
                new List<KeyboardButton> { new KeyboardButton("Скачать обработанный файл в JSON"), new KeyboardButton("Скачать обработанный файл в СSV") }
            }
        );
    }

    private static async Task DownloadData(ITelegramBotClient botClient, Update update)
    {
        string fileName = update.Message.Document.FileName;
        if (fileName.EndsWith(".csv"))
        {
            pathFile = await CSVProcessing.Download(botClient, update);
            table = CSVProcessing.Read(pathFile, out List<int> bugs);
            if (bugs.Count > 0)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Обнаружены ошибки в {bugs.Count} строках, они были пропущены при записи", replyMarkup: GetButtons());
            }
            CSVProcessing.Write(table, pathFile);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Данные загружены!");
        }
        else if (fileName.EndsWith(".json"))
        {
            pathFile = await JSONProcessing.Download(botClient, update);
            table = JSONProcessing.Read(pathFile);
            JSONProcessing.Write(pathFile, table);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Данные загружены!");
        }
        else
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Программа не поддерживает такой тип данных!");
        }

    }
    private async void ProcessUpdateAsync(ITelegramBotClient botClient, Update update)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message) {
            var command = update.Message.Text;
            if (table is null)
            {
                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    await DownloadData(botClient, update);
                } else
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Для других функции для начала загрузите данные из файла!");
                }
            }
            else
            {
                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    await DownloadData(botClient, update);
                }
                else if (command == "Фильтрация по Type") {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Фильтрация по Type", replyMarkup: GetButtons());
                    List<GeraldicSign> editedTable = await FilteringData.FilterByOneConditionAsync(botClient, update, "Type", table);
                }
                else if (command == "Фильтрация по RegistrationDate")
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Фильтрация по RegistrationDate", replyMarkup: GetButtons());
                    List<GeraldicSign> editedTable = await FilteringData.FilterByOneConditionAsync(botClient, update, "RegistrationDate", table);
                }
                else if (command == "Фильтрация по CertificateHolderName и RegistrationDate")
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Фильтрация по CertificateHolderName и RegistrationDate", replyMarkup: GetButtons());
                    List<GeraldicSign> editedTable = await FilteringData.FilterByTwoConditionsAsync(botClient, update, "CertificateHolderName", "RegistrationDate", table);
                }
                else if (command == "Сортировка по возрастанию")
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Сортировка по возрастанию", replyMarkup: GetButtons());
                    List<GeraldicSign> editedTable = SortingData.SortByRegistrationNumber(table);
                }
                else if (command == "Сортировка по убыванию")
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Сортировка по убыванию", replyMarkup: GetButtons());
                    List<GeraldicSign> editedTable = SortingData.SortByRegistrationNumber(table, true);
                }
                else if (command == "Скачать обработанный файл в JSON")
                {
                    string pathNewFile = $"C:\\Programming\\C#\\GIT\\GIT_Module-3_KDZ-3\\data\\LastOutput.json";
                    JSONProcessing.Write(pathNewFile, table);
                    await JSONProcessing.Upload(botClient, update, pathNewFile);
                }
                else if (command == "Скачать обработанный файл в СSV")
                {
                    string pathNewFile = $"C:\\Programming\\C#\\GIT\\GIT_Module-3_KDZ-3\\data\\LastOutput.csv";
                    CSVProcessing.Write(table, pathNewFile);
                    await CSVProcessing.Upload(botClient, update, pathNewFile);
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message.Chat.Id, "Такой функции нет", replyMarkup: GetButtons());
                }
            }
        } 
        else {
            await client.SendTextMessageAsync(update.Message.Chat.Id, update.Type + " Not implemented!");
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
