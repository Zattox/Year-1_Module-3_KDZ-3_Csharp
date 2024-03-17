using Telegram.Bot;
using Telegram.Bot.Types;
public class FilteringData 
{
    private static async Task<string> FindValueSelectionAsync(ITelegramBotClient botClient, long chatId, string condition)
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

    public static async Task FilterByOneConditionAsync(ITelegramBotClient botClient, Update update, string condition, List<GeraldicSign> table)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        string selection = await FindValueSelectionAsync(botClient, chatId, condition);

        List<GeraldicSign> newTable = new List<GeraldicSign>();
        newTable.Add(table[0]); newTable.Add(table[1]);

        foreach (GeraldicSign row in table)
        {
            string infoCondition = FindInfoCondition(condition, row);
            if (infoCondition == selection)
            {
                newTable.Add(row);
            }
        }

        if (newTable.Count == 2)
        {
            await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "По данному значению не нашлось результатов :(");
            return;
        }
        return;
    }
    public static async Task FilterByTwoConditionsAsync(ITelegramBotClient botClient, Update update, string firstCondition, string secondCondition, List<GeraldicSign> table)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        string firstSelection = await FindValueSelectionAsync(botClient, chatId, firstCondition);
        string secondSelection = await FindValueSelectionAsync(botClient, chatId, secondCondition);

        List<GeraldicSign> newTable = new List<GeraldicSign>();
        newTable.Add(table[0]); newTable.Add(table[1]);

        foreach (GeraldicSign row in table)
        {
            string firstInfoCondition = FindInfoCondition(firstCondition, row);
            string secondInfoCondition = FindInfoCondition(secondCondition, row);
            if (firstInfoCondition == firstSelection && secondInfoCondition == secondSelection)
            {
                newTable.Add(row);
            }
        }

        if (newTable.Count == 2)
        {
            await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "По данным значениям не нашлось результатов :(");
            return;
        }
        return;
    }
}