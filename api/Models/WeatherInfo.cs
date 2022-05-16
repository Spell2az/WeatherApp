namespace api.Models;

public class WeatherInfo
{
    public string LocationName { get; set; } = String.Empty;
    public string WeatherMain { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Icon { get; set; } = String.Empty;
    public float CurrentTemp { get; set; }
    public float MaximumTemp { get; set; }
    public float MinimumTemp { get; set; }
    public int Pressure { get; set; }
    public int Humidity{ get; set; }
    public int Sunrise { get; set; }
    public int Sunset { get; set; }
}
