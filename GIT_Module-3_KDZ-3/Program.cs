internal class Program
{
    private const string token = "6757896301:AAGpTPCVQBwt0dkJN5wYG4MD3dCmmUJWGyg";
    static void Main(string[] args)
    {
        try
        {
            TelegramBotHelper hlp = new TelegramBotHelper(token);
            hlp.GetUpdates();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

