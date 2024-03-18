using Microsoft.Extensions.Logging;
internal class Program
{
    private const string token = "6757896301:AAGpTPCVQBwt0dkJN5wYG4MD3dCmmUJWGyg";
    static void Main(string[] args)
    {
        try
        {
            TelegramBotHelper hlp = new TelegramBotHelper(token); // Иницилизация класса для взаимодействия с ботом.
            Methods log = new Methods(); // Иникилизация класса для логированния.
            hlp.GetUpdates();
        }
        catch (Exception ex)
        {
            Methods.WriteErrorLog(nameof(Main), ex);
        }
    }
}

