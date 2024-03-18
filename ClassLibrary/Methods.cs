using System.Diagnostics;
public class Methods
{
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
    /// <summary>
    /// Меняет два объекта одного типа местами.
    /// </summary>
    /// <param name="a">Первый объект.</param>
    /// <param name="b">Второй объект.</param>
    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    /// <summary>
    /// Вывод текста в консоль с выбранным цветом.
    /// </summary>
    /// <param name="text">Текс.</param>
    /// <param name="color">Цвет букв в консоли.</param>
    public static void PrintWithColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
