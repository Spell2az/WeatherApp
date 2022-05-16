using System.Text.Json.Serialization;

namespace api.Models;

public class WeatherInfoDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("main")]
    public WeatherData WeatherMainData { get; set; }

    [JsonPropertyName("weather")]
    public Weather[] WeatherMain { get; set; }

    [JsonPropertyName("sys")]
    public Sys SysData { get; set; }
    public class WeatherData
    {
        [JsonPropertyName("temp")]
        public float Temperature { get; set; }
        [JsonPropertyName("temp_min")]
        public float Minimum { get; set; }
        [JsonPropertyName("temp_max")]
        public float Maximum { get; set; }
        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }
        [JsonPropertyName("humidity")]
        public int Humidity{ get; set; }
    }
    public class Weather
    {
        [JsonPropertyName("main")]
        public string MainWeather { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }

    public class Sys
    {
        [JsonPropertyName("sunrise")]
        public int Sunrise { get; set; }
        [JsonPropertyName("sunset")]
        public int Sunset { get; set; }
    }
}

