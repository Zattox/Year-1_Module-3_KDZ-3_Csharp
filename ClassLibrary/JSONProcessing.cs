using System.Text.Json;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Text;
using System.Text.Encodings.Web;
using static AppConstants;
public class JSONProcessing
{
    private static int countOfOutput = 1;

    /// <summary>
    /// Чтение данных из уже загруженного JSON файла.
    /// </summary>
    /// <param name="stream">Поток до существуещего JSON файла.</param>
    /// <returns>Считанные данные в виде List<GeraldicSign>.</returns>
    public static List<GeraldicSign> Read(Stream stream)
    {
        Methods.WriteStartLog(nameof(Read));

        string text = "";
        TextReader oldIn = Console.In;
        using (StreamReader sr = new StreamReader(stream))
        {
            Console.SetIn(sr);
            text = sr.ReadToEnd();
        }
        Console.SetIn(oldIn);

        var table = JsonSerializer.Deserialize<List<GeraldicSign>>(text);
        stream.Close();

        Methods.WriteStopLog(nameof(Read));
        return table;
    }

    /// <summary>
    /// Запись данных из таблицы в JSON файл.
    /// </summary>
    /// <param name="table">Таблица с данными.</param>
    /// <returns>Поток до JSON файла.</returns>
    public static Stream Write(List<GeraldicSign> table)
    {
        Methods.WriteStartLog(nameof(Write));

        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };
        string jsonString = JsonSerializer.Serialize(table, options);

        string destinationFilePath = OutputJSONPath + $"\\output{countOfOutput++}.json";
        Stream fileStream = System.IO.File.Create(destinationFilePath);
        TextWriter oldOut = Console.Out;
        using (StreamWriter sw = new StreamWriter(fileStream, Encoding.UTF8))
        {
            Console.SetOut(sw);
            Console.Write(jsonString);
        }
        Console.SetOut(oldOut);

        fileStream.Close();

        Methods.WriteStopLog(nameof(Write));
        return new FileStream(destinationFilePath, FileMode.Open);
    }

    /// <summary>
    /// Скачивание JSON файла из телеграмм чата с ботом.
    /// </summary>
    /// <param name="botClient">Обозначение нужного чата с выбранным телеграмм ботом.</param>
    /// <param name="update">Последнее сообщение пользователя из этого чата.</param>
    /// <returns>Поток до скаченного файла.</returns>
    public static async Task<Stream> Download(ITelegramBotClient botClient, Update update)
    {
        Methods.WriteStartLog(nameof(Download));

        var fileId = update.Message.Document.FileId;
        string destinationFilePath = DataPath + $"\\LastInput.json";

        Stream fileStream = System.IO.File.Create(destinationFilePath);
        await botClient.GetInfoAndDownloadFileAsync(fileId: fileId, destination: fileStream);
        fileStream.Close();

        Methods.WriteStopLog(nameof(Download));
        return new FileStream(destinationFilePath, FileMode.Open);
    }

    /// <summary>
    /// Отправка JSON файла в телеграмм чат с ботом.
    /// </summary>
    /// <param name="botClient">Обозначение нужного чата с выбранным телеграмм ботом.</param>
    /// <param name="update">Последнее сообщение пользователя из этого чата.</param>
    /// <param name="stream">Поток файла, который нужно отправить.</param>
    public static async Task Upload(ITelegramBotClient botClient, Update update, Stream stream)
    {
        Methods.WriteStartLog(nameof(Upload));

        await botClient.SendDocumentAsync(
            chatId: update.Message.Chat.Id,
            document: InputFile.FromStream(stream: stream, fileName: $"Table.json"),
            replyMarkup: Buttons.GetMenuButtons());

        Methods.WriteStopLog(nameof(Upload));
    }
}
