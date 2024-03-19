using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using static AppConstants;
public class CSVProcessing
{
    private static int countOfOutput = 1;

    /// <summary>
    /// Удаляет ненужные символы из строки.
    /// </summary>
    /// <param name="line">Строка, которую нужно отформатировать.</param>
    /// <returns>Отформатированная строка.</returns>
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
    /// <summary>
    /// Убирает лишние пустые строки.
    /// </summary>
    /// <param name="data">Изначальный набор строк.</param>
    /// <returns>Набор строк длинной равной количеству заголовков.</returns>
    private static List<string> DataCorrection(string[] data)
    {
        List<string> result = new List<string>();
        foreach (string line in data)
        {
            string newLine = RemoveExtraCharacters(line);
            if (result.Count < CountOfHeaders)
            {
                result.Add(newLine);
            }
        }
        return result;
    }
    /// <summary>
    /// Проверка правильности типов полей строки.
    /// </summary>
    /// <param name="row">Строка в которой нужно проверить правильность типов.</param>
    /// <returns>true - если прошла проверку, иначе false.</returns>
    private static bool CheckDataCsvFile(List<string> row)
    {
        for (int i = 0; i < CountOfHeaders; ++i)
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
    /// <summary>
    /// Чтение данных из уже загруженного CSV файла.
    /// </summary>
    /// <param name="filePath">Путь до существуещего CSV файла.</param>
    /// <param name="bugs">Список индексов некорректных строчек.</param>
    /// <returns>Считанные данные в виде List<GeraldicSign>.</returns>
    /// <exception cref="ArgumentNullException">Пустые заголовки файла.</exception>
    /// <exception cref="ArgumentException">Неверное количество заголовков файла.</exception>
    public static List<GeraldicSign> Read(Stream stream, out List<int> bugs)
    {
        Methods.WriteStartLog(nameof(Read));

        List<GeraldicSign> table = new List<GeraldicSign>();
        bugs = new List<int>(0);
        using (StreamReader sr = new StreamReader(stream))
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

            if (headersEng.Count != CountOfHeaders || headersRus.Count != CountOfHeaders)
            {
                throw new ArgumentException("Неверное количество заголовков");
            }

            int indOfLine = 3;
            List<List<string>> stringData = new List<List<string>>();
            while ((curLine = sr.ReadLine()) is not null)
            {
                List<string> values = new List<string>(DataCorrection(curLine.Split(Separator)));
                if (values.Count == CountOfHeaders && CheckDataCsvFile(values))
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
        stream.Close();

        Methods.WriteStopLog(nameof(Read));
        return table;
    }
    /// <summary>
    /// Запись данных из таблицы в CSV файл.
    /// </summary>
    /// <param name="table">Таблица с данными.</param>
    /// <param name="path">Путь до выходного файла.</param>
    public static Stream Write(List<GeraldicSign> table)
    {
        Methods.WriteStartLog(nameof(Write));

        string destinationFilePath = OutputCSVPath + $"\\output{countOfOutput++}.csv";
        Stream fileStream = System.IO.File.Create(destinationFilePath);
        using (var sw = new StreamWriter(fileStream, Encoding.UTF8))
        {
            foreach (var elem in table)
            {
                sw.WriteLine(elem.ToString());
            }
        }
        fileStream.Close();

        Methods.WriteStopLog(nameof(Write));
        return new FileStream(destinationFilePath, FileMode.Open);
    }
    /// <summary>
    /// Скачивание CSV файла из телеграмм чата с ботом.
    /// </summary>
    /// <param name="botClient">Обозначение нужного чата с выбранным телеграмм ботом.</param>
    /// <param name="update">Последнее сообщение пользователя из этого чата.</param>
    /// <param name="ExecutablePath">Путь до директории куда необходимо скачать файл.</param>
    /// <returns>Абсолютный путь до скаченного файла.</returns>
    public static async Task<Stream> Download(ITelegramBotClient botClient, Update update)
    {
        Methods.WriteStartLog(nameof(Download));

        var fileId = update.Message.Document.FileId;
        string destinationFilePath = DataPath + $"\\LastInput.csv";

        Stream fileStream = System.IO.File.Create(destinationFilePath);
        await botClient.GetInfoAndDownloadFileAsync(fileId: fileId, destination: fileStream);
        fileStream.Close();

        Methods.WriteStopLog(nameof(Download));
        return new FileStream(destinationFilePath, FileMode.Open);
    }
    /// <summary>
    /// Отправка CSV файла в телеграмм чат с ботом.
    /// </summary>
    /// <param name="botClient">Обозначение нужного чата с выбранным телеграмм ботом.</param>
    /// <param name="update">Последнее сообщение пользователя из этого чата.</param>
    /// <param name="path">Абсолютный путь до файла, который нужно отправить.</param>
    public static async Task Upload(ITelegramBotClient botClient, Update update, Stream stream)
    {
        Methods.WriteStartLog(nameof(Upload));

        Message message = await botClient.SendDocumentAsync(
            chatId: update.Message.Chat.Id,
            document: InputFile.FromStream(stream: stream, fileName: $"Table.csv"),
            replyMarkup: Buttons.GetMenuButtons());

        Methods.WriteStopLog(nameof(Upload));
    }
}