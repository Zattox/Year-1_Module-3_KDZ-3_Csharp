﻿using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static AppConstants;
public class Methods
{
    private readonly static ILogger<Methods> logger;
    private readonly static string logFilePath;

    /// <summary>
    /// Статический конструктор для иницилизации логгера и создания нужных директорий.
    /// </summary>
    static Methods()
    {
        CreateDirectories();

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
    /// Поиск абсолютного пути до директории на уровне папки проекта.
    /// </summary>
    /// <returns>Абсолютный путь до директории на уровне папки проекта.</returns>
    public static void CreateDirectories()
    {
        // Поиск абсолютного пути до запускаемого файла проекта.
        Process currentProcess = Process.GetCurrentProcess();
        ExecutablePath = currentProcess.MainModule.FileName;

        // Поднятие вверх на нужное количетсво директорий.
        ExecutablePath = Path.GetDirectoryName(ExecutablePath);
        ExecutablePath = Path.GetDirectoryName(ExecutablePath);
        ExecutablePath = Path.GetDirectoryName(ExecutablePath);
        ExecutablePath = Path.GetDirectoryName(ExecutablePath);
        ExecutablePath = Path.GetDirectoryName(ExecutablePath);

        // Создание директории \data. (Хранение всех файлов с данными)
        DataPath = ExecutablePath + "\\data";
        if (!Directory.Exists(DataPath))
        {
            Directory.CreateDirectory(DataPath);
        }

        // Создание директории \data\JSONOutput. (Хранение файлов для вывода данных с .json)
        OutputJSONPath = DataPath + "\\JSONOutput";
        if (!Directory.Exists(OutputJSONPath))
        {
            Directory.CreateDirectory(OutputJSONPath);
        }

        // Создание директории \data\CSVOutput.(Хранение файлов для вывода данных с .csv)
        OutputCSVPath = DataPath + "\\CSVOutput";
        if (!Directory.Exists(OutputCSVPath))
        {
            Directory.CreateDirectory(OutputCSVPath);
        }

        // Создание директории \var.(Хранение файлов для вывода логирования)
        LogPath = ExecutablePath + "\\var";
        if (!Directory.Exists(LogPath))
        {
            Directory.CreateDirectory(LogPath);
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
