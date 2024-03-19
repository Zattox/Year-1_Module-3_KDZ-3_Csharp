using Telegram.Bot.Types.ReplyMarkups;
using static AppConstants;
public class Buttons
{
    /// <summary>
    /// Отрисовывает кликабельные кнопки в телеграмм чате необходимые для выбора режима сортировки.
    /// </summary>
    internal static IReplyMarkup? GetSortingButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton(SortingButtonText1) },
                new List<KeyboardButton> { new KeyboardButton(SortingButtonText2) }
            }
        );
    }
    /// <summary>
    /// Отрисовывает кликабельные кнопки в телеграмм чате необходимые для выбора режима вывода данных.
    /// </summary>
    internal static IReplyMarkup? GetOutputButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton(OutputButtonText1) },
                new List<KeyboardButton> { new KeyboardButton(OutputButtonText2) }
            }
        );
    }
    /// <summary>
    /// Отрисовывает кликабельные кнопки в телеграмм чате необходимые для выбора команды бота.
    /// </summary>
    internal static IReplyMarkup? GetMenuButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton(MenuButtonText1) },
                new List<KeyboardButton> { new KeyboardButton(MenuButtonText2) },
                new List<KeyboardButton> { new KeyboardButton(MenuButtonText3) },
                new List<KeyboardButton> { new KeyboardButton(MenuButtonText4) }
            }
        );
    }
}