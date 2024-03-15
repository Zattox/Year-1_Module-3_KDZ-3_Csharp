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
    internal static GeraldicSignList SortByRegistrationNumber(GeraldicSignList data, bool flag = false)
    {
        GeraldicSignList sortedData = data;
        sortedData.Data.Sort(Comparator);

        if (flag)
            sortedData.Data.Reverse();

        return sortedData;
    }
}
