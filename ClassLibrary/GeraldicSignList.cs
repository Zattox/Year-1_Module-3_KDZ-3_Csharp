using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
public class GeraldicSignList: IEnumerable
{
    private const int countOfHeaders = 9;
    private List<string> headersEng;
    private List<string> headersRus;
    private List<GeraldicSign> data;

    public GeraldicSignList()
    {
        headersEng = new List<string>(countOfHeaders);
        headersRus = new List<string>(countOfHeaders);
        data = new List<GeraldicSign>();
    }
    public GeraldicSignList(List<string> headersEng, List<string> headersRus, List<GeraldicSign> data)
    {
        this.headersEng = headersEng;
        this.headersRus = headersRus;
        this.data = data;
    }
    [JsonPropertyName("HeadersEng")]
    public List<string> HeadersEng => headersEng;

    [JsonPropertyName("HeadersRus")]
    public List<string> HeadersRus => headersRus;

    [JsonPropertyName("Data")]
    public List<GeraldicSign> Data { get { return data; } set { data = value; } }

    public static int CountOfHeaders => countOfHeaders;
    public IEnumerator GetEnumerator() => data.GetEnumerator();
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
    public override string ToString()
    {
        string result = string.Empty;
        foreach (string header in headersEng)
        {
            result += $"\"{header}\";";
        }
        foreach (string header in headersRus)
        {
            result += $"\"{header}\";";
        }
        foreach (GeraldicSign elem in data)
        {
            result += elem.ToString();
        }

        return result;
    }
}
