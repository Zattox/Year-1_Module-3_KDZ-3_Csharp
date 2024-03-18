public class FilteringData 
{  
    private static void FindValueFiterOneCondition(string message, out string condition, out string value)
    {
        string text = "Фильтрация по ";
        string[] arr = message.Remove(0, text.Length).Split('\"');
        var result = arr.Where(x => x.Length > 1).ToList();
        condition = result[0];
        value = result[1];
    }
    public static List<GeraldicSign> FilterByOneCondition(List<GeraldicSign> table, string message)
    {
        Methods.WriteStartLog(nameof(FilterByOneCondition));
        FindValueFiterOneCondition(message, out string condition, out string value);
        List<GeraldicSign> result = new List<GeraldicSign>(table);
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

        result.Insert(0, table[1]);
        result.Insert(0, table[0]);

        Methods.WriteStopLog(nameof(FilterByOneCondition));
        return result;
    }
    private static void FindValueFiterTwoCondition(string message, string buttonText, out string value1, out string value2)
    {
        string[] arr = message.Remove(0, buttonText.Length).Split('\"');
        var result = arr.Where(x => x.Length > 1).ToList();
        value1 = result[0];
        value2 = result[1];
    }
    public static List<GeraldicSign> FilterByTwoConditions(List<GeraldicSign> table, string message, string buttonText)
    {
        Methods.WriteStartLog(nameof(FilterByTwoConditions));
        FindValueFiterTwoCondition(message, buttonText, out string value1, out string value2);

        List<GeraldicSign> result = new List<GeraldicSign>(table);
        result.Remove(table[0]);
        result.Remove(table[1]);

        result = result.Where(row => row.CertificateHolderName == value1 && row.RegistrationDate == value2).ToList();

        result.Insert(0, table[0]);
        result.Insert(1, table[1]);

        Methods.WriteStopLog(nameof(FilterByTwoConditions));
        return result;
    }
}