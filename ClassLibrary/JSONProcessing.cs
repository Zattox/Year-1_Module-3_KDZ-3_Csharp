using System.Text.Json;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Text;
using System.Text.Encodings.Web;
public class JSONProcessing
{
    public static List<GeraldicSign> Read(string filePath)
    {
        Methods.WriteStartLog(nameof(Read));
        string text = "";
        TextReader oldIn = Console.In;
        using (StreamReader sr = new StreamReader(filePath))
        {
            Console.SetIn(sr);
            text = sr.ReadToEnd();
        }
        Console.SetIn(oldIn);
        
        var table = JsonSerializer.Deserialize<List<GeraldicSign>>(text);
        Methods.WriteStopLog(nameof(Read));
        return table;
    }
    public static void Write(string filePath, List<GeraldicSign> table)
    {
        Methods.WriteStartLog(nameof(Write));
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
        Methods.WriteStopLog(nameof(Write));
    }
    public static async Task<string> Download(ITelegramBotClient botClient, Update update, string ExecutablePath)
    {
        Methods.WriteStartLog(nameof(Download));
        var fileId = update.Message.Document.FileId;
        string destinationFilePath = $"{ExecutablePath}\\LastInput.json";

        await using Stream fileStream = System.IO.File.Create(destinationFilePath);
        await botClient.GetInfoAndDownloadFileAsync(fileId: fileId, destination: fileStream);
        fileStream.Close();
        Methods.WriteStopLog(nameof(Download));
        return destinationFilePath;
    }
    public static async Task Upload(ITelegramBotClient botClient, Update update, string path)
    {
        Methods.WriteStartLog(nameof(Upload));
        await using Stream stream = System.IO.File.OpenRead(path);
        await botClient.SendDocumentAsync(
            chatId: update.Message.Chat.Id,
            document: InputFile.FromStream(stream: stream, fileName: $"Table.json"),
            replyMarkup: Buttons.GetMenuButtons());
        Methods.WriteStopLog(nameof(Upload));
    }
}
