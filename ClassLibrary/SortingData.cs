public class SortingData
{
    /// <summary>
    /// Выделение номера как пары чисел из строки вида "M x..\y...".
    /// </summary>
    /// <param name="firstNumber">Первое число из пары.</param>
    /// <param name="secondNumber">Второк число из пары.</param>
    /// <param name="registrationNumber">Строка с регистрационным номером.</param>
    private static void ParseRegistrationNumber(out int firstNumber, out int secondNumber, string registrationNumber)
    {
        string strFirstNumber = string.Empty;
        string strSecondNumber = string.Empty;

        int i = 0;
        while (i < registrationNumber.Length && !char.IsDigit(registrationNumber[i]))
        {
            ++i;
        }
        while (i < registrationNumber.Length && char.IsDigit(registrationNumber[i]))
        {
            strFirstNumber += registrationNumber[i++];
        }

        ++i;
        while (i < registrationNumber.Length && char.IsDigit(registrationNumber[i]))
        {
            strSecondNumber += registrationNumber[i++];
        }

        firstNumber = int.Parse(strFirstNumber);
        if (strSecondNumber.Length == 0)
            strSecondNumber = "0";
        secondNumber = int.Parse(strSecondNumber);
    }
    /// <summary>
    /// Собственный компаратор для сортировки List<GeraldicSign>.
    /// </summary>
    /// <param name="x">Первый какой-то элементы из списка.</param>
    /// <param name="y">Второй какой-то элементы из списка.</param>
    /// <returns>Возвращает 0 если элементы равны. Возвращает -1, если x меньше y. Возвращает 1, если x больше y.</returns>
    private static int Comparator(GeraldicSign x, GeraldicSign y)
    {
        ParseRegistrationNumber(out int firstRowX, out int firstRowY, x.RegistrationNumber);
        ParseRegistrationNumber(out int secondRowX, out int secondRowY, y.RegistrationNumber);

        if (firstRowX == secondRowX)
        {
            if (firstRowY > secondRowY)
            {
                return 1;
            }
            else if (firstRowY < secondRowY)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        } 
        else if (firstRowX > secondRowX)
        {
            return 1;
        } 
        else
        { 
            return -1;
        }
    }

    /// <summary>
    /// Сортировка таблицы.
    /// </summary>
    /// <param name="data">Таблица для сортировки.</param>
    /// <param name="flag">false - прямой порядок сортировки, true - обратный порядок сортировки.</param>
    /// <returns>Отсортированную таблицу.</returns>
    public static List<GeraldicSign> SortByRegistrationNumber(List<GeraldicSign> data, bool flag = false)
    {
        Methods.WriteStartLog(nameof(SortByRegistrationNumber));
        List<GeraldicSign> sortedData = new List<GeraldicSign>(data);

        // Убираем заголовки из таблицы.
        sortedData.Remove(data[0]);
        sortedData.Remove(data[1]);

        sortedData.Sort(Comparator);

        if (flag)
            sortedData.Reverse();

        // Возвращаем заголовки в таблицу.
        sortedData.Insert(0, data[1]);
        sortedData.Insert(0, data[0]);

        Methods.WriteStopLog(nameof(SortByRegistrationNumber));
        return sortedData;
    }
}