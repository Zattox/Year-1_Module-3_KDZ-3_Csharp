using Telegram.Bot.Types.ReplyMarkups;
using static AppConstants;
internal class Buttons
{
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

