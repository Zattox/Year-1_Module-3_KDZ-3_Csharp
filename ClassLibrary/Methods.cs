using Microsoft.Extensions.Logging;
using static AppConstants;
public class Methods
{
    private readonly static ILogger<Methods> logger;
    private readonly static string logFilePath;

    /// <summary>
    /// Статический конструктор для иницилизации логера и создания, нужных для работы программы, директорий.
    /// </summary>
    static Methods()
    {
        logFilePath += LogPath + $"\\Console_log_{DateTime.Now:dd-MM}.txt";
        if (!File.Exists(logFilePath))
        {
            var st = File.Create(logFilePath);
            st.Close();
        }

        using (StreamWriter logFileWriter = new StreamWriter(logFilePath, append: true))
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            logger = loggerFactory.CreateLogger<Methods>();
        }
    }

    /// <summary>
    /// Запись отметки о старте работы метода.
    /// </summary>
    /// <param name="methodName">Название метода, который начал работу.</param>
    public static void WriteStartLog(string methodName)
    {
        string text = $"{methodName} request at {DateTime.Now:HH:mm:ss}";
        logger.LogInformation(text);
        using (StreamWriter logFileWriter = new StreamWriter(logFilePath, append: true))
        {
            logFileWriter.WriteLine(text);
        }
    }

    /// <summary>
    /// Запись отметки о конце работы метода.
    /// </summary>
    /// <param name="methodName">Название метода, который закончил работу.</param>
    public static void WriteStopLog(string methodName)
    {
        string text = $"{methodName} successfully completed at {DateTime.Now:HH:mm:ss}";
        logger.LogInformation(text);
        using (StreamWriter logFileWriter = new StreamWriter(logFilePath, append: true))
        {
            logFileWriter.WriteLine(text);
        }
    }

    /// <summary>
    /// Запись отметки об ошибке в программе.
    /// </summary>
    /// <param name="methodName">Название метода, в котором произошла ошибка.</param>
    /// <param name="ex">Произошедшая ошибка.</param>
    public static void WriteErrorLog(string methodName, Exception ex)
    {
        string text = $"An error occurred in {methodName} at {DateTime.Now:HH:mm:ss}\n{ex.GetType()}\n{ex.Message}";
        logger.LogError(text);
        using (StreamWriter logFileWriter = new StreamWriter(logFilePath, append: true))
        {
            logFileWriter.WriteLine(text);
        }
    }
}
