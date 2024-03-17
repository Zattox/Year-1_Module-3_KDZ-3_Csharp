using System.Text.Json;
using Telegram.Bot.Types;
using Telegram.Bot;
internal class JSONProcessing
{
    internal static List<GeraldicSign> Read(string filePath)
    {
        string text = string.Empty;
        TextReader oldIn = Console.In;
        using (StringReader sr = new StringReader(filePath))
        {
            Console.SetIn(sr);
            text = sr.ReadToEnd();
        }
        Console.SetIn(oldIn);
        List<GeraldicSign> data = JsonSerializer.Deserialize<List<GeraldicSign>>(text);
        
        return data;
    }
    internal static void Write(string filePath, List<GeraldicSign> table)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(table, options);

        TextWriter oldOut = Console.Out;
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            Console.SetOut(sw);
            Console.Write(jsonString);
        }
        Console.SetOut(oldOut);
    }
    internal static async Task<string> Download(ITelegramBotClient botClient, Update update)
    {
        var fileId = update.Message.Document.FileId;
        string destinationFilePath = $"C:\\Programming\\C#\\GIT\\GIT_Module-3_KDZ-3\\data\\{fileId}.json";

        await using Stream fileStream = System.IO.File.Create(destinationFilePath);
        await botClient.GetInfoAndDownloadFileAsync(fileId: fileId, destination: fileStream);
        fileStream.Close();
        return destinationFilePath;
    }

    internal static async Task Upload(ITelegramBotClient botClient, Update update, string path)
    {
        await using Stream stream = System.IO.File.OpenRead(path);
        Message message = await botClient.SendDocumentAsync(
            chatId: update.Message.Chat.Id,
            document: InputFile.FromStream(stream: stream, fileName: $"table.json"));
    }
}
