internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            TelegramBotHelper hlp = new TelegramBotHelper(AppConstants.Token); // Иницилизация класса для взаимодействия с ботом.
            Methods log = new Methods(); // Иницилизация класса для логированния.
            hlp.GetUpdates();
        }
        catch (Exception ex)
        {
            Methods.WriteErrorLog(nameof(Main), ex);
        }
    }
}