using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
public class Methods
{
    private readonly static ILogger<Methods> logger;
    private readonly static string logFilePath;
    static Methods()
    {
        logFilePath = FindExecutablePath();
        logFilePath = Path.GetDirectoryName(logFilePath);

        logFilePath += "\\var";
        if (!Directory.Exists(logFilePath))
        {
            Directory.CreateDirectory(logFilePath);
        }
        logFilePath +=$"\\Console_log_{DateTime.Now:dd-MM}.txt";
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
    public static string FindExecutablePath()
    {
        Process currentProcess = Process.GetCurrentProcess();
        string executablePath = currentProcess.MainModule.FileName;

        executablePath = Path.GetDirectoryName(executablePath);
        executablePath = Path.GetDirectoryName(executablePath);
        executablePath = Path.GetDirectoryName(executablePath);
        executablePath = Path.GetDirectoryName(executablePath);
        executablePath = Path.GetDirectoryName(executablePath);

        executablePath += "\\data";
        if (!Directory.Exists(executablePath))
        {
            Directory.CreateDirectory(executablePath);
        }

        return executablePath;
    }

    public static void WriteStartLog(string methodName)
    {
        string text = $"{methodName} request at {DateTime.Now:HH:mm:ss}";
        logger.LogInformation(text);
        using (StreamWriter logFileWriter = new StreamWriter(logFilePath, append: true))
        {
            logFileWriter.WriteLine(text);
        }
    }
    public static void WriteStopLog(string methodName) {
        string text = $"{methodName} successfully completed at {DateTime.Now:HH:mm:ss}";
        logger.LogInformation(text);
        using (StreamWriter logFileWriter = new StreamWriter(logFilePath, append: true))
        {
            logFileWriter.WriteLine(text);
        }
    }

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
