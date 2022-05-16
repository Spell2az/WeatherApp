using System.Text.Json.Serialization;

namespace api.Models;

public class Coordinates
{
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
}
