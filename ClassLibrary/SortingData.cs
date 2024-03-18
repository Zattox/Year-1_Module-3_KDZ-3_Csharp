public class SortingData
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
        if (strSecondNumber.Length == 0)
            strSecondNumber = "0";
        secondNumber = int.Parse(strSecondNumber);
    }
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
    public static List<GeraldicSign> SortByRegistrationNumber(List<GeraldicSign> data, bool flag = false)
    {
        Methods.WriteStartLog(nameof(SortByRegistrationNumber));
        List<GeraldicSign> sortedData = new List<GeraldicSign>(data);
        sortedData.Remove(data[0]);
        sortedData.Remove(data[1]);

        sortedData.Sort(Comparator);

        List<string> list1 = new List<string>();
        foreach(var elem in data)
        {
            list1.Add(elem.RegistrationNumber);
        }

        List<string> list2 = new List<string>();
        foreach (var elem in sortedData)
        {
            list2.Add(elem.RegistrationNumber);
        }

        if (flag)
            sortedData.Reverse();

        sortedData.Insert(0, data[1]);
        sortedData.Insert(0, data[0]);
        Methods.WriteStopLog(nameof(SortByRegistrationNumber));
        return sortedData;
    }
}