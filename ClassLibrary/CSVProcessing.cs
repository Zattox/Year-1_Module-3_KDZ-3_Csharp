using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
internal class CSVProcessing
{
    const string Separator = "\";\"";
    private static string RemoveExtraCharacters(string line)
    {
        string answer = string.Empty;
        for (int i = 0; i < line.Length; ++i)
        {
            if (line[i] != '\"' && line[i] != ';')
            {
                answer += line[i];
            }
        }
        return answer;
    }
    private static List<string> DataCorrection(string[] data)
    {
        List<string> result = new List<string>();
        foreach (string line in data)
        {
            string newLine = RemoveExtraCharacters(line);
            if (result.Count < TelegramBotHelper.CountOfHeaders)
            {
                result.Add(newLine);
            }
        }
        return result;
    }
    private static bool CheckDataCsvFile(List<string> row)
    {
        for (int i = 0; i < TelegramBotHelper.CountOfHeaders; ++i)
        {
            if (row[i] is null || row[i] == "NA")
            {
                continue;
            }
            switch (i)
            {
                case 6:
                    if (!DateTime.TryParse(row[i], out DateTime _))
                    {
                        return false;
                    }
                    break;

                case 8:
                    if (!int.TryParse(row[i], out int _))
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

    internal static List<GeraldicSign> Read(string filePath, out List<int> bugs)
    {
        List<GeraldicSign> table = new List<GeraldicSign>();
        bugs = new List<int>(0);
        using (StreamReader sr = new StreamReader(filePath))
        {
            string curLine = sr.ReadLine();
            List<string> headersEng = new List<string>(DataCorrection(curLine.Split(Separator)));
            if (headersEng is null || headersEng.Count == 0)
            {
                throw new ArgumentNullException("Пустые английские заголовки");
            }

            curLine = sr.ReadLine();
            List<string> headersRus = new List<string>(DataCorrection(curLine.Split(Separator)));
            if (headersRus is null || headersRus.Count == 0)
            {
                throw new ArgumentNullException("Пустые русские заголовки");
            }

            if (headersEng.Count != TelegramBotHelper.CountOfHeaders || headersRus.Count != TelegramBotHelper.CountOfHeaders)
            {
                throw new ArgumentException("Неверное количество заголовков");
            }

            int indOfLine = 3;
            List<List<string>> stringData = new List<List<string>>();
            while ((curLine = sr.ReadLine()) is not null)
            {
                List<string> values = new List<string>(DataCorrection(curLine.Split(Separator)));
                if (values.Count == TelegramBotHelper.CountOfHeaders && CheckDataCsvFile(values))
                {
                    stringData.Add(values);
                }
                else
                {
                    bugs.Add(indOfLine);
                }
                ++indOfLine;
            }

            table.Add(new GeraldicSign(headersEng));
            table.Add(new GeraldicSign(headersRus));    
            foreach (var row in stringData)
            {
                table.Add(new GeraldicSign(row));
            }
        }

        return table;
    }
    internal static void Write(List<GeraldicSign> table, string path)
    {
        using (var sw = new StreamWriter(path, false, Encoding.UTF8))
        {
            foreach (var elem in table)
            {
                sw.WriteLine(elem.ToString());
            }    
        }
    }
    internal static async Task<string> Download(ITelegramBotClient botClient, Update update)
    {
        var fileId = update.Message.Document.FileId;
        string destinationFilePath = $"C:\\Programming\\C#\\GIT\\GIT_Module-3_KDZ-3\\data\\LastInput.csv";

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
            document: InputFile.FromStream(stream: stream, fileName: $"Table.csv"));
    }
}