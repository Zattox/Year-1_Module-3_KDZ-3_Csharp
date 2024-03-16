using Telegram.Bot;
using Telegram.Bot.Types;
public class FilteringData 
{
    private static async Task<string> FindValueSelection(ITelegramBotClient botClient, long chatId, string condition)
    {
        string selection = string.Empty;
        while (true)
        {
            var curMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Введите значение поля для построения выборки {condition}: ");
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
        return selection;
    }
    private static string FindInfoCondition(string condition, GeraldicSign row)
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
            "Global_id" => row.GlobalId,
            _ => ""
        };
        return infoCondition;
    }

    public static async Task FilterByOneCondition(ITelegramBotClient botClient, Update update, string condition, GeraldicSignList table)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        string selection = await FindValueSelection(botClient, chatId, condition);

        GeraldicSignList newTable = new GeraldicSignList(table.HeadersEng, table.HeadersRus, new List<GeraldicSign>(0));
        foreach (GeraldicSign row in table)
        {
            string infoCondition = FindInfoCondition(condition, row);
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
        string firstSelection = await FindValueSelection(botClient, chatId, firstCondition);
        string secondSelection = await FindValueSelection(botClient, chatId, secondCondition);

        GeraldicSignList newTable = new GeraldicSignList(table.HeadersEng, table.HeadersRus, new List<GeraldicSign>());
        foreach (GeraldicSign row in table)
        {
            string firstInfoCondition = FindInfoCondition(firstCondition, row);
            string secondInfoCondition = FindInfoCondition(secondCondition, row);
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