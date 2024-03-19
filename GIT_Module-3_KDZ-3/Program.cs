/*
  Студент:     Тепляков Владислав Витальевич  
  Группа:      БПИ2310-2
  Задача:      Контрольное домашнее задание 3, модуль 3
  Вариант:     8
*/
internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            TelegramBotHelper hlp = new TelegramBotHelper(AppConstants.Token); // Иницилизация класса для взаимодействия с ботом.
            Methods log = new Methods(); // Иницилизация класса для логированния.
            hlp.GetUpdates(); // Старт работы с ботом.
        }
        catch (Exception ex)
        {
            Methods.WriteErrorLog(nameof(Main), ex);
        }
    }
}