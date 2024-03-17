﻿using System.Text.Json;
using System.Text.Json.Serialization;
public class GeraldicSign
{
    private const int countOfHeaders = 9;

    private string name;
    private string type;
    private string picture;
    private string description;
    private string semantics;
    private string certificateHolderName;
    private string registrationDate;
    private string registrationNumber;
    private string global_id;
    public GeraldicSign()
    {
        name = string.Empty;
        type = string.Empty;
        picture = string.Empty;
        description = string.Empty;
        semantics = string.Empty;
        certificateHolderName = string.Empty;
        registrationDate = string.Empty;
        registrationNumber = string.Empty;
        global_id = string.Empty;
    }
    public GeraldicSign(string name, string type, string picture, string description,
        string semantics, string certificateHolderName, string registrationDate,
        string registrationNumber, string global_id)
    {
        this.name = name;
        this.type = type;
        this.picture = picture;
        this.description = description;
        this.semantics = semantics;
        this.certificateHolderName = certificateHolderName;
        this.registrationDate = registrationDate;
        this.registrationNumber = registrationNumber;
        this.global_id = global_id;
    }
    public GeraldicSign(List<string> data)
    {
        name = data[0];
        type = data[1];
        picture = data[2];
        description = data[3];
        semantics = data[4];
        certificateHolderName = data[5];
        registrationDate = data[6];
        registrationNumber = data[7];
        global_id = data[8];
    }

    [JsonPropertyName("Name")]
    public string Name => name;

    [JsonPropertyName("Type")]
    public string Type => type;

    [JsonPropertyName("Picture")]
    public string Picture => picture;

    [JsonPropertyName("Description")]
    public string Description => description;

    [JsonPropertyName("Semantics")]
    public string Semantics => semantics;

    [JsonPropertyName("CertificateHolderName")]
    public string CertificateHolderName => certificateHolderName;

    [JsonPropertyName("RegistrationDate")]
    public string RegistrationDate => registrationDate;

    [JsonPropertyName("RegistrationNumber")]
    public string RegistrationNumber => registrationNumber;

    [JsonPropertyName("global_id")]
    public string GlobalId => global_id;
    public static int CountOfHeaders => countOfHeaders;
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
    public override string ToString()
    {
        string result = $"\"{name}\";" + $"\"{type}\";" + $"\"{picture}\";" + $"\"{description}\";" 
            + $"\"{semantics}\";" + $"\"{certificateHolderName}\";" + $"\"{registrationDate}\";"
            + $"\"{registrationNumber}\";" + $"\"{global_id}\";";
        return result;
    }
}
