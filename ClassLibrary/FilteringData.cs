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

    internal static async Task<List<GeraldicSign>> FilterByOneConditionAsync(ITelegramBotClient botClient, Update update, string condition, List<GeraldicSign> table)
    {
        var chatId = update.Message.Chat.Id;
        string selection = await FindValueSelectionAsync(botClient, chatId, condition);

        List<GeraldicSign> newTable = new List<GeraldicSign>();
        foreach (GeraldicSign row in table)
        {
            string infoCondition = FindInfoCondition(condition, row);
            if (infoCondition == selection)
            {
                newTable.Add(row);
            }
        }

        if (newTable.Count == 0)
        {
            await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "По данным значениям не нашлось результатов :(");
        }
        else
        {
            newTable.Insert(0, table[1]);
            newTable.Insert(0, table[0]);
        }
        return newTable;
    }
    internal static async Task<List<GeraldicSign>> FilterByTwoConditionsAsync(ITelegramBotClient botClient, Update update, string firstCondition, string secondCondition, List<GeraldicSign> table)
    {
        var chatId = update.Message.Chat.Id;
        string firstSelection = await FindValueSelectionAsync(botClient, chatId, firstCondition);
        string secondSelection = await FindValueSelectionAsync(botClient, chatId, secondCondition);

        List<GeraldicSign> newTable = new List<GeraldicSign>();

        foreach (GeraldicSign row in table)
        {
            string firstInfoCondition = FindInfoCondition(firstCondition, row);
            string secondInfoCondition = FindInfoCondition(secondCondition, row);
            if (firstInfoCondition == firstSelection && secondInfoCondition == secondSelection)
            {
                newTable.Add(row);
            }
        }

        if (newTable.Count == 0)
        {
            await botClient.SendTextMessageAsync(
                   chatId: chatId,
                   text: "По данным значениям не нашлось результатов :(");
        } else
        {
            newTable.Insert(0, table[1]); 
            newTable.Insert(0, table[0]);
        }
        return newTable;
    }
}