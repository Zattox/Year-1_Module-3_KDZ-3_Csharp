using Microsoft.VisualBasic;
using System.ComponentModel.Design;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
public class TelegramBotHelper
{
    private static string ExecutablePath = Methods.FindExecutablePath();

    private string token;
    private static string pathFile;
    private static List<GeraldicSign> table;
    private static TelegramBotClient botClient;

    public TelegramBotHelper(string token)
    {
        this.token = token;
    }
    private static async Task DownloadData(Update update)
    {
        string fileName = update.Message.Document.FileName;
        if (fileName.EndsWith(".csv"))
        {
            pathFile = await CSVProcessing.Download(botClient, update, ExecutablePath);
            table = CSVProcessing.Read(pathFile, out List<int> bugs);
            if (bugs.Count > 0)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Обнаружены ошибки в {bugs.Count} строках, они были пропущены при записи", replyMarkup: Buttons.GetMenuButtons());
            }
            CSVProcessing.Write(table, pathFile);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Данные загружены!");
        }
        else if (fileName.EndsWith(".json"))
        {
            pathFile = await JSONProcessing.Download(botClient, update, ExecutablePath);
            table = JSONProcessing.Read(pathFile);
            JSONProcessing.Write(pathFile, table);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Данные загружены!");
        }
        else
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Программа не поддерживает такой тип данных!");
        }

    }
    private async void ProcessUpdateAsync(Update update)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message) {
            var command = update.Message.Text;
            if (table is null)
            {
                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    await DownloadData(update);
                } 
                else
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Для других функции для начала загрузите файла c данными!");
                }
            }
            else
            {
                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    await DownloadData(update);
                }
                switch (command)
                {
                    case "Загрузить новый файл":
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Загрузите файл в формате JSON или CSV");
                            break;
                        }

                    case "Фильтрация по Type":
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Фильтрация по Type", replyMarkup: Buttons.GetMenuButtons());
                            List<GeraldicSign> editedTable = await FilteringData.FilterByOneConditionAsync(botClient, update, "Type", table);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            break;
                        }

                    case "Фильтрация по RegistrationDate":
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Фильтрация по RegistrationDate", replyMarkup: Buttons.GetMenuButtons());
                            List<GeraldicSign> editedTable = await FilteringData.FilterByOneConditionAsync(botClient, update, "RegistrationDate", table);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            break;
                        }

                    case "Фильтрация по CertificateHolderName и RegistrationDate":
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Фильтрация по CertificateHolderName и RegistrationDate", replyMarkup: Buttons.GetMenuButtons());
                            List<GeraldicSign> editedTable = await FilteringData.FilterByTwoConditionsAsync(botClient, update, "CertificateHolderName", "RegistrationDate", table);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            break;
                        }

                    case "Сортировка по возрастанию":
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Сортировка по возрастанию", replyMarkup: Buttons.GetMenuButtons());
                            List<GeraldicSign> editedTable = SortingData.SortByRegistrationNumber(table);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            break;
                        }

                    case "Сортировка по убыванию":
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Сортировка по убыванию", replyMarkup: Buttons.GetMenuButtons());
                            List<GeraldicSign> editedTable = SortingData.SortByRegistrationNumber(table, true);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            break;
                        }

                    case "Скачать обработанный файл в JSON":
                        {
                            await JSONProcessing.Upload(botClient, update, $"{ExecutablePath}\\LastOutput.json");
                            break;
                        }

                    case "Скачать обработанный файл в СSV":
                        {
                            await CSVProcessing.Upload(botClient, update, $"{ExecutablePath}\\LastOutput.csv");
                            break;
                        }

                    default:
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Такой функции нет", replyMarkup: Buttons.GetMenuButtons());
                        break;
                }
            }
        } 
        else {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, update.Type + " Not implemented!");
        }
    }

    public void GetUpdates()
    {
        botClient = new TelegramBotClient(token);
        var me = botClient.GetMeAsync().Result;
        if (me != null && !string.IsNullOrEmpty(me.Username))
        {
            int offset = 0;
            while (true)
            {
                try
                {
                    var updates = botClient.GetUpdatesAsync(offset).Result;
                    if (updates != null && updates.Count() > 0)
                    {
                        foreach (var update in updates)
                        {
                            ProcessUpdateAsync(update);
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
