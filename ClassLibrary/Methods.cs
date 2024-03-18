using System.Diagnostics;
internal class Methods
{
    internal static string FindExecutablePath()
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
}
