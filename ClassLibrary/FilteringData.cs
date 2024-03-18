public class FilteringData 
{  
    private static void FindValueFiterOneCondition(string message, out string condition, out string value)
    {
        string text = "Фильтрация по ";
        string[] arr = message.Remove(0, text.Length).Split(' ', '\"');
        var result = arr.Where(x => !string.IsNullOrEmpty(x)).ToList();
        condition = result[0];
        value = result[1];
    }
    internal static List<GeraldicSign> FilterByOneCondition(List<GeraldicSign> table, string message)
    {
        FindValueFiterOneCondition(message, out string condition, out string value);
        List<GeraldicSign> result = condition switch
        {
            "Name" => table.Where(row => row.Name == value).ToList(),
            "Type" => table.Where(row => row.Type == value).ToList(),
            "Picture" => table.Where(row => row.Picture == value).ToList(),
            "Description" => table.Where(row => row.Description == value).ToList(),
            "Semantics" => table.Where(row => row.Semantics == value).ToList(),
            "CertificateHolderName" => table.Where(row => row.CertificateHolderName == value).ToList(),
            "RegistrationDate" => table.Where(row => row.RegistrationDate == value).ToList(),
            "RegistrationNumber" => table.Where(row => row.RegistrationNumber == value).ToList(),
            "Global_id" => table.Where(row => row.Global_id == value).ToList(),
            _ => new List<GeraldicSign>()
        }; 
        return result;
    }
    private static void FindValueFiterTwoCondition(string message, string buttonText, out string value1, out string value2)
    {
        string[] arr = message.Remove(0, buttonText.Length).Split(' ', '\"');
        var result = arr.Where(x => !string.IsNullOrEmpty(x)).ToList();
        value1 = result[2];
        value2 = result[3];
    }
    internal static List<GeraldicSign> FilterByTwoConditions(List<GeraldicSign> table, string message, string buttonText)
    {
        FindValueFiterTwoCondition(message, buttonText, out string value1, out string value2);
        List<GeraldicSign> result = table.Where(row => row.CertificateHolderName == value1 && row.RegistrationDate == value2).ToList();
        return result;
    }
}