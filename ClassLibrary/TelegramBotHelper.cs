using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types;
using static AppConstants;
public class TelegramBotHelper
{
    private string token; // Токен для обращения к боту.
    private static Stream lastJsonDownload, lastCsvDownload;
    private static Stream lastJsonUpload, lastCsvUpload;
    private static List<GeraldicSign> table; // Таблица с данными из последнего загруженного файла.
    private static TelegramBotClient botClient;
    public TelegramBotHelper(string token)
    {
        this.token = token;
    }
    /// <summary>
    /// Скачивание данных от пользователя.
    /// </summary>
    /// <param name="update">Сообщение пользователя.</param>
    private async Task DownloadData(Update update)
    {
        Methods.WriteStartLog(nameof(DownloadData));

        string fileName = update.Message.Document.FileName;
        if (fileName.EndsWith(".csv"))
        {
            lastCsvDownload = await CSVProcessing.Download(botClient, update);
            table = CSVProcessing.Read(lastCsvDownload, out List<int> bugs);
            if (bugs.Count > 0)
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Обнаружены ошибки в {bugs.Count} строках, они были пропущены при записи", replyMarkup: Buttons.GetMenuButtons());
            lastCsvUpload = CSVProcessing.Write(table);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
        }
        else if (fileName.EndsWith(".json"))
        {
            lastJsonDownload = await JSONProcessing.Download(botClient, update);
            table = JSONProcessing.Read(lastJsonDownload);
            lastJsonUpload = JSONProcessing.Write(table);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
        }
        else
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, TypeErrorMessage);
        }

        Methods.WriteStopLog(nameof(DownloadData));
    }
    /// <summary>
    /// Сохранение последних отредактированных данных. 
    /// </summary>
    /// <param name="update">Сообщение от пользователя.</param>
    /// <param name="editedTable">Отредактированная таблица.</param>
    private async Task CompleteEditingTask(Update update, List<GeraldicSign> editedTable)
    {
        Methods.WriteStartLog(nameof(CompleteEditingTask));
        if (editedTable.Count == 2)
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, UnluckyMessage, replyMarkup: Buttons.GetMenuButtons());
        }
        else
        {
            lastCsvUpload = CSVProcessing.Write(editedTable);
            lastJsonUpload = JSONProcessing.Write(editedTable);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
        }
        Methods.WriteStopLog(nameof(CompleteEditingTask));
    }
    /// <summary>
    /// Обработка сообщений пользователя.
    /// </summary>
    /// <param name="update">Сообщение пользователя.</param>
    private async void ProcessUpdateAsync(Update update)
    {
        Methods.WriteStartLog(nameof(ProcessUpdateAsync));
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var command = update.Message.Text;
            if (table is null) // Поведение программы, если пользователь еще не загрузил таблицу с данными.
            {
                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    try
                    {
                        await DownloadData(update);
                    }
                    catch (Exception ex)
                    {
                        Methods.WriteErrorLog(nameof(ProcessUpdateAsync), ex);
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, ErrorMessage + ex.Message);
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, StoppedMessage);
                }
            }
            else // Поведение программы, если пользователь загрузил таблицу с данными.
            {
                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document) // Если сообщение - документ.
                {
                    await DownloadData(update);
                    return;
                }

                // Если пользователь хочет произвести выборку, ему необходимо в нужном формате прислать запрос.
                if (command.StartsWith(FilterButtonText1))
                {
                    List<GeraldicSign> editedTable = FilteringData.FilterByOneCondition(table, command);
                    await CompleteEditingTask(update, editedTable);
                    return;
                }
                else if (command.StartsWith(FilterButtonText2))
                {
                    List<GeraldicSign> editedTable = FilteringData.FilterByOneCondition(table, command);
                    await CompleteEditingTask(update, editedTable);
                    return;
                }
                else if (command.StartsWith(FilterButtonText3))
                {
                    List<GeraldicSign> editedTable = FilteringData.FilterByTwoConditions(table, command);
                    await CompleteEditingTask(update, editedTable);
                    return;
                }

                // Основное меню команд, если в таблице есть данные.
                switch (command)
                {
                    case MenuButtonText1:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, InputMessage, replyMarkup: Buttons.GetMenuButtons());
                            break;
                        }

                    case MenuButtonText2:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, ChooseFilterMessage);
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

                    case SortingButtonText1:
                        {
                            List<GeraldicSign> editedTable = SortingData.SortByRegistrationNumber(table);
                            await CompleteEditingTask(update, editedTable);
                            break;
                        }

                    case SortingButtonText2:
                        {
                            List<GeraldicSign> editedTable = SortingData.SortByRegistrationNumber(table, true);
                            await CompleteEditingTask(update, editedTable);
                            break;
                        }

                    case OutputButtonText1:
                        {
                            await JSONProcessing.Upload(botClient, update, lastJsonUpload);
                            break;
                        }

                    case OutputButtonText2:
                        {
                            await CSVProcessing.Upload(botClient, update, lastCsvUpload);
                            break;
                        }

                    default:
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, CommandErrorMessage, replyMarkup: Buttons.GetMenuButtons());
                        break;
                }
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, TypeErrorMessage);
        }
        Methods.WriteStopLog(nameof(ProcessUpdateAsync));
    }
    /// <summary>
    /// Обновление сообщений от пользователя.
    /// </summary>
    public void GetUpdates()
    {
        Methods.WriteStartLog(nameof(GetUpdates));

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
                    Methods.WriteErrorLog(nameof(GetUpdates), ex);
                }
                Thread.Sleep(1000);
            }
        }

        Methods.WriteStopLog(nameof(GetUpdates));
    }
}