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
    internal static List<GeraldicSign> FilterByOneCondition(List<GeraldicSign> table, string message)
    {
        FindValueFiterOneCondition(message, out string condition, out string value);
        List<GeraldicSign> result = new List<GeraldicSign>(table);
        result.Remove(table[0]);
        result.Remove(table[0]);

        result = condition switch
        {
            "Name" => result.Where(row => row.Name == value).ToList(),
            "Type" => result.Where(row => row.Type == value).ToList(),
            "Picture" => result.Where(row => row.Picture == value).ToList(),
            "Description" => result.Where(row => row.Description == value).ToList(),
            "Semantics" => result.Where(row => row.Semantics == value).ToList(),
            "CertificateHolderName" => result.Where(row => row.CertificateHolderName == value).ToList(),
            "RegistrationDate" => result.Where(row => row.RegistrationDate == value).ToList(),
            "RegistrationNumber" => result.Where(row => row.RegistrationNumber == value).ToList(),
            "Global_id" => result.Where(row => row.Global_id == value).ToList(),
            _ => new List<GeraldicSign>()
        };

        result.Insert(0, table[1]);
        result.Insert(0, table[0]);

        return result;
    }
    private static void FindValueFiterTwoCondition(string message, string buttonText, out string value1, out string value2)
    {
        string[] arr = message.Remove(0, buttonText.Length).Split('\"');
        var result = arr.Where(x => x.Length > 1).ToList();
        value1 = result[0];
        value2 = result[1];
    }
    internal static List<GeraldicSign> FilterByTwoConditions(List<GeraldicSign> table, string message, string buttonText)
    {
        FindValueFiterTwoCondition(message, buttonText, out string value1, out string value2);

        List<GeraldicSign> result = new List<GeraldicSign>(table);
        result.Remove(table[0]);
        result.Remove(table[0]);

        result = result.Where(row => row.CertificateHolderName == value1 && row.RegistrationDate == value2).ToList();

        result.Insert(0, table[0]);
        result.Insert(1, table[1]);
        return result;
    }
}