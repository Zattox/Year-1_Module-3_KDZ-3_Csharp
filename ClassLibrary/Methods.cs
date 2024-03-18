using Microsoft.Extensions.Logging;
using System.Diagnostics;
public class Methods
{
    private readonly static ILogger<Methods> logger;
    static Methods()
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        logger = factory.CreateLogger<Methods>();
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
        logger.LogInformation($"{methodName} request at {DateTime.Now:hh:mm:ss}");
    }
    public static void WriteStopLog(string methodName) { 
        logger.LogInformation($"{methodName} successfully completed at {DateTime.Now:hh:mm:ss}");
    }

    public static void WriteErrorLog(string methodName, Exception ex)
    {
        logger.LogError($"An error occurred in {methodName} at {DateTime.Now:hh:mm:ss}\n{ex.GetType()}\n{ex.Message}");
    }
}
