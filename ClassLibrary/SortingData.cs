internal class SortingData
{
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
        secondNumber = int.Parse(strSecondNumber);
    }
    private static int Comparator(GeraldicSign firstRow, GeraldicSign secondRow)
    {
        ParseRegistrationNumber(out int firstRowX, out int firstRowY, firstRow.RegistrationNumber);
        ParseRegistrationNumber(out int secondRowX, out int secondRowY, secondRow.RegistrationNumber);

        if (firstRowX == secondRowX)
            return Convert.ToInt32(firstRowY > secondRowY);
        else
            return Convert.ToInt32(firstRowX > secondRowX);
    }
    internal static List<GeraldicSign> SortByRegistrationNumber(List<GeraldicSign> data, bool flag = false)
    {
        List<GeraldicSign> sortedData = data;
        sortedData.Remove(data[0]);
        sortedData.Remove(data[1]);
        sortedData.Sort(Comparator);

        if (flag)
            sortedData.Reverse();

        sortedData.Insert(0, data[1]);
        sortedData.Insert(0, data[0]);

        return sortedData;
    }
}
