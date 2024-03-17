﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
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
            if (result.Count < GeraldicSignList.CountOfHeaders)
            {
                result.Add(newLine);
            }
        }
        return result;
    }
    private static bool CheckDataCsvFile(List<string> row)
    {
        for (int i = 0; i < GeraldicSignList.CountOfHeaders; ++i)
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

    internal static GeraldicSignList ReadCsvFile(string destinationFilePath, out List<int> bugs)
    {
        GeraldicSignList table;
        bugs = new List<int>(0);
        using (StreamReader sr = new StreamReader(destinationFilePath))
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

            if (headersEng.Count != GeraldicSignList.CountOfHeaders || headersRus.Count != GeraldicSignList.CountOfHeaders)
            {
                throw new ArgumentException("Неверное количество заголовков");
            }

            int indOfLine = 3;
            List<List<string>> stringData = new List<List<string>>();
            while ((curLine = sr.ReadLine()) is not null)
            {
                List<string> values = new List<string>(DataCorrection(curLine.Split(Separator)));
                if (values.Count == GeraldicSignList.CountOfHeaders && CheckDataCsvFile(values))
                {
                    stringData.Add(values);
                }
                else
                {
                    bugs.Add(indOfLine);
                }
                ++indOfLine;
            }

            List<GeraldicSign> data = new List<GeraldicSign>();
            foreach (var row in stringData)
            {
                data.Add(new GeraldicSign(row));
            }

            table = new GeraldicSignList(headersEng, headersRus, data);
        }

        return table;
    }
    internal static void SaveCsvFile(ITelegramBotClient botClient, Update update, GeraldicSignList table, string path)
    {
        if (path == null || path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
        {
            Console.WriteLine("Путь указан некорректно");
        }

        using (var sw = new StreamWriter(path))
        {
            GeraldicSign headersEng = new GeraldicSign(table.HeadersEng);
            GeraldicSign headersRus = new GeraldicSign(table.HeadersRus);

            sw.WriteLine(headersEng.ToString());
            sw.WriteLine(headersRus.ToString());
            foreach (var elem in table.Data)
            {
                sw.WriteLine(elem.ToString());
            }    
        }
    }
    internal static async Task<string> DownloadCsvFile(ITelegramBotClient botClient, Update update)
    {
        var fileId = update.Message.Document.FileId;
        string destinationFilePath = $"C:\\Programming\\C#\\GIT\\GIT_Module-3_KDZ-3\\data\\{fileId}.csv";

        await using Stream fileStream = System.IO.File.Create(destinationFilePath);
        await botClient.GetInfoAndDownloadFileAsync(fileId: fileId, destination: fileStream);
        fileStream.Close();
        return destinationFilePath;
    }

    internal static async Task UploadCsvFile(ITelegramBotClient botClient, Update update, string path)
    {
        await using Stream stream = System.IO.File.OpenRead(path);
        Message message = await botClient.SendDocumentAsync(
            chatId: update.Message.Chat.Id,
            document: InputFile.FromStream(stream: stream, fileName: $"table.csv"));
    }
}