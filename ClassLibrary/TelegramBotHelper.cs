using Telegram.Bot;
using Telegram.Bot.Types;
using static AppConstants;
public class TelegramBotHelper
{
    private string token;
    private static string pathFile;
    private static List<GeraldicSign> table;
    private static TelegramBotClient botClient;
    private static string ExecutablePath = Methods.FindExecutablePath();

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
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Обнаружены ошибки в {bugs.Count} строках, они были пропущены при записи", replyMarkup: Buttons.GetMenuButtons());
            CSVProcessing.Write(table, pathFile);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
        }
        else if (fileName.EndsWith(".json"))
        {
            pathFile = await JSONProcessing.Download(botClient, update, ExecutablePath);
            table = JSONProcessing.Read(pathFile);
            JSONProcessing.Write(pathFile, table);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
        }
        else
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, TypeErrorMessage);
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
                    try
                    {
                        await DownloadData(update);
                    }
                    catch (Exception ex)
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, ErrorMessage + ex.Message);
                    }
                } 
                else
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, StoppedMessage);
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
                    case MenuButtonText1:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, InputMessage, replyMarkup: Buttons.GetMenuButtons());
                            break;
                        }

                    case MenuButtonText2:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, ChooseCommandMessage, replyMarkup: Buttons.GetFilterButtons());
                            break;
                        }

                    case MenuButtonText3:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, ChooseCommandMessage, replyMarkup: Buttons.GetSortingButtons());
                            break;
                        }

                    case MenuButtonText4:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, ChooseCommandMessage, replyMarkup: Buttons.GetOutputButtons());
                            break;
                        }

                    case FilterButtonText1:
                        {
                            List<GeraldicSign> editedTable = await FilteringData.FilterByOneConditionAsync(botClient, update, "Type", table);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
                            break;
                        }

                    case FilterButtonText2:
                        {
                            List<GeraldicSign> editedTable = await FilteringData.FilterByOneConditionAsync(botClient, update, "RegistrationDate", table);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
                            break;
                        }

                    case FilterButtonText3:
                        {
                            List<GeraldicSign> editedTable = await FilteringData.FilterByTwoConditionsAsync(botClient, update, "CertificateHolderName", "RegistrationDate", table);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
                            break;
                        }

                    case SortingButtonText1:
                        {
                            List<GeraldicSign> editedTable = SortingData.SortByRegistrationNumber(table);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
                            break;
                        }

                    case SortingButtonText2:
                        {
                            List<GeraldicSign> editedTable = SortingData.SortByRegistrationNumber(table, true);
                            CSVProcessing.Write(editedTable, $"{ExecutablePath}\\LastOutput.csv");
                            JSONProcessing.Write($"{ExecutablePath}\\LastOutput.json", editedTable);
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
                            break;
                        }

                    case OutputButtonText1:
                        {
                            await JSONProcessing.Upload(botClient, update, $"{ExecutablePath}\\LastOutput.json");
                            break;
                        }

                    case OutputButtonText2:
                        {
                            await CSVProcessing.Upload(botClient, update, $"{ExecutablePath}\\LastOutput.csv");
                            break;
                        }

                    default:
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, CommandErrorMessage, replyMarkup: Buttons.GetMenuButtons());
                        break;
                }
            }
        } 
        else {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, TypeErrorMessage);
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
