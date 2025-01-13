using System.Text.Json.Serialization;

namespace MauiApp3;

public class RatesJson 
{
    public DateTime Date { get; set; }
    public DateTime PreviousDate { get; set; }
    public string PreviousURL { get; set; } = "";
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("Valute")]
    public Dictionary<string, Currency> Rates { get; set; } = new Dictionary<string, Currency>();
}
