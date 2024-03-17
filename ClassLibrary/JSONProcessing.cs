using System.Text.Json;
internal class JSONProcessing
{
    internal static List<GeraldicSign> Read(string path)
    {
        string text = string.Empty;
        TextReader oldIn = Console.In;
        using (var sr = new StringReader(path))
        {
            Console.SetIn(sr);
            text = sr.ReadToEnd();
        }
        Console.SetIn(oldIn);
        
        List<GeraldicSign> data = JsonSerializer.Deserialize<List<GeraldicSign>>(text);
        return data;
    }
}
