using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Collections.Specialized.BitVector32;
public class FilteringData 
{
    public static async Task FilterByOneCondition(ITelegramBotClient botClient, Update update, string condition, GeraldicSignList table)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        string selection = string.Empty;
        while (true)
        {
            var curMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Введите значение поля для построения выборки {condition}");
            if (curMessage.Text is null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Вы ввели пустое значение, повторите попытку");
                continue;
            }
            selection = curMessage.Text;
            break;
        }

        GeraldicSignList newTable = new GeraldicSignList(table.HeadersEng, table.HeadersRus, new List<GeraldicSign>(0));
        foreach (GeraldicSign row in table)
        {
            string infoCondition = condition switch
            {
                "Name" => row.Name,
                "Type" => row.Type,
                "Picture" => row.Picture,
                "Description" => row.Description,
                "Semantics" => row.Semantics,
                "CertificateHolderName" => row.CertificateHolderName,
                "RegistrationDate" => row.RegistrationDate,
                "RegistrationNumber" => row.RegistrationNumber,
                "Global_id" => row.GlobalId
            };
            if (infoCondition == selection)
            {
                newTable.Data.Add(row);
            }
        }

        if (newTable.Data.Count == 0)
        {
            await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "По данному значению не нашлось результатов :(");
            return;
        }
        return;
    }
    public static async Task FilterByTwoConditions(ITelegramBotClient botClient, Update update, string firstCondition, string secondCondition, GeraldicSignList table)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        string firstSelection = string.Empty;
        while (true)
        {
            var curMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Введите значение поля для построения выборки {firstCondition}");
            if (curMessage.Text is null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Вы ввели пустое значение, повторите попытку");
                continue;
            }
            firstSelection = curMessage.Text;
            break;
        }

        string secondSelection = string.Empty;
        while (true)
        {
            var curMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Введите значение поля для построения выборки {secondCondition}");
            if (curMessage.Text is null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Вы ввели пустое значение, повторите попытку");
                continue;
            }
            secondSelection = curMessage.Text;
            break;
        }

        GeraldicSignList newTable = new GeraldicSignList(table.HeadersEng, table.HeadersRus, new List<GeraldicSign>());
        foreach (GeraldicSign row in table)
        {
            string firstInfoCondition = firstCondition switch
            {
                "Name" => row.Name,
                "Type" => row.Type,
                "Picture" => row.Picture,
                "Description" => row.Description,
                "Semantics" => row.Semantics,
                "CertificateHolderName" => row.CertificateHolderName,
                "RegistrationDate" => row.RegistrationDate,
                "RegistrationNumber" => row.RegistrationNumber,
                "Global_id" => row.GlobalId
            };
            string secondInfoCondition = secondCondition switch
            {
                "Name" => row.Name,
                "Type" => row.Type,
                "Picture" => row.Picture,
                "Description" => row.Description,
                "Semantics" => row.Semantics,
                "CertificateHolderName" => row.CertificateHolderName,
                "RegistrationDate" => row.RegistrationDate,
                "RegistrationNumber" => row.RegistrationNumber,
                "Global_id" => row.GlobalId
            };

            if (firstInfoCondition == firstSelection && secondInfoCondition == secondSelection)
            {
                newTable.Data.Add(row);
            }
        }

        if (newTable.Data.Count == 0)
        {
            await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "По данным значениям не нашлось результатов :(");
            return;
        }
        return;
    }

}