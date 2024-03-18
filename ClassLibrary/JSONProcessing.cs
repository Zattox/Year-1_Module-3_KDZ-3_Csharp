using System.Text.Json;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Text;
using System.Text.Encodings.Web;
internal class JSONProcessing
{
    internal static List<GeraldicSign> Read(string filePath)
    {
        string text = "";
        TextReader oldIn = Console.In;
        using (StreamReader sr = new StreamReader(filePath))
        {
            Console.SetIn(sr);
            text = sr.ReadToEnd();
        }
        Console.SetIn(oldIn);
        
        var table = JsonSerializer.Deserialize<List<GeraldicSign>>(text);
        return table;
    }
    internal static void Write(string filePath, List<GeraldicSign> table)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true 
        };
        string jsonString = JsonSerializer.Serialize(table, options);

        TextWriter oldOut = Console.Out;
        using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            Console.SetOut(sw);
            Console.Write(jsonString);
        }
        Console.SetOut(oldOut);
    }
    internal static async Task<string> Download(ITelegramBotClient botClient, Update update, string ExecutablePath)
    {
        var fileId = update.Message.Document.FileId;
        string destinationFilePath = $"{ExecutablePath}\\LastInput.json";

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
            document: InputFile.FromStream(stream: stream, fileName: $"Table.json"));
    }
}
