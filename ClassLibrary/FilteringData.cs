public class FilteringData
{
    /// <summary>
    /// Разделение строки по заданному формату.
    /// </summary>
    /// <param name="message">Строка для разделение.</param>
    /// <param name="condition">Условие для выборки, полученное из строки.</param>
    /// <param name="value">Значение поля для выборки, полученнок из строки.</param>
    private static void FindValueFiterOneCondition(string message, out string condition, out string value)
    {
        string text = "Фильтрация по ";
        string[] arr = message.Remove(0, text.Length).Split('\"');
        var result = arr.Where(x => x.Length > 1).ToList();
        condition = result[0];
        value = result[1];
    }

    /// <summary>
    /// Выборка по одному условию.
    /// </summary>
    /// <param name="table">Таблица с данными.</param>
    /// <param name="message">Сообщение, содержащие данные о формате выборки.</param>
    /// <returns>Отфильтрованная таблица.</returns>
    public static List<GeraldicSign> FilterByOneCondition(List<GeraldicSign> table, string message)
    {
        Methods.WriteStartLog(nameof(FilterByOneCondition));

        List<GeraldicSign> result = new List<GeraldicSign>(table);
        FindValueFiterOneCondition(message, out string condition, out string value);

        // Убираем заголовки из таблицы.
        result.Remove(table[0]);
        result.Remove(table[1]);

        if (condition.StartsWith("Type"))
        {
            result = result.Where(row => row.Type == value).ToList();
        }
        else if (condition.StartsWith("RegistrationDate"))
        {
            result = result.Where(row => row.RegistrationDate == value).ToList();
        }

        // Возвращаем заголовки в таблицу.
        result.Insert(0, table[1]);
        result.Insert(0, table[0]);

        Methods.WriteStopLog(nameof(FilterByOneCondition));
        return result;
    }

    /// <summary>
    /// Разделение строки по заданному формату.
    /// </summary>
    /// <param name="message">Строка для разделение.</param>
    /// <param name="formatText">Текст для определения формата строки.</param>
    /// <param name="value1">Значение поля для первого условия.</param>
    /// <param name="value2">Значение поля для второго условия.</param>
    private static void FindValueFiterTwoCondition(string message, string formatText, out string value1, out string value2)
    {
        string[] arr = message.Remove(0, formatText.Length).Split('\"');
        var result = arr.Where(x => x.Length > 1).ToList();
        value1 = result[0];
        value2 = result[1];
    }

    /// <summary>
    /// Выборка по двум условиям.
    /// </summary>
    /// <param name="table">Таблица с данными.</param>
    /// <param name="message">Сообщение, содержащие данные о формате выборки.</param>
    /// <returns>Отфильтрованная таблица</returns>
    public static List<GeraldicSign> FilterByTwoConditions(List<GeraldicSign> table, string message)
    {
        Methods.WriteStartLog(nameof(FilterByTwoConditions));

        List<GeraldicSign> result = new List<GeraldicSign>(table);
        FindValueFiterTwoCondition(message, AppConstants.FilterButtonText3, out string value1, out string value2);

        // Убираем заголовки из таблицы.
        result.Remove(table[0]);
        result.Remove(table[1]);

        result = result.Where(row => row.CertificateHolderName == value1 && row.RegistrationDate == value2).ToList();

        // Возвращаем заголовки в таблицу.
        result.Insert(0, table[0]);
        result.Insert(1, table[1]);

        Methods.WriteStopLog(nameof(FilterByTwoConditions));
        return result;
    }
}