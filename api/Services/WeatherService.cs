using System.Text.Json;
using api.Models;
using FluentResults;

namespace api.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ICoordinateTranslationService coordinateTranslationService;
        private readonly IConfiguration configuration;

        public WeatherService(IHttpClientFactory clientFactory, ICoordinateTranslationService coordinateTranslationService, IConfiguration configuration)
        {
            this.clientFactory = clientFactory;
            this.coordinateTranslationService = coordinateTranslationService;
            this.configuration = configuration;
        }
        public async Task<Result<WeatherInfo>> GetWeatherInfo(string location)
        {
            var apiKey = configuration["ApiKey"];
            var coordinates = await coordinateTranslationService.GetCoordinatesFromLocation(location);
            if (coordinates.IsFailed)
            {
                return Result.Fail("Failed to get coordinates");
            }

            var client = clientFactory.CreateClient("WeatherApiClient");

            using var response = await client.GetAsync($"/data/2.5/weather?lat={coordinates.Value.Latitude}&lon={coordinates.Value.Longitude}&appid={apiKey}&units=metric");

            if (!response.IsSuccessStatusCode)
            {
                return Result.Fail("Failed to get weather info");
            }

            var data = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<WeatherInfoDto>(data);
            if (dto == null)
            {
                return Result.Fail("Failed to deserialize weather response");
            }

            var dtoWeather = dto.WeatherMain.First();
            var weatherInfo = new WeatherInfo
            {
                LocationName = dto.Name,
                WeatherMain = dtoWeather.MainWeather,
                Description = dtoWeather.Description,
                Icon = dtoWeather.Icon,
                CurrentTemp = dto.WeatherMainData.Temperature,
                MaximumTemp = dto.WeatherMainData.Maximum,
                MinimumTemp = dto.WeatherMainData.Minimum,
                Pressure = dto.WeatherMainData.Pressure,
                Humidity = dto.WeatherMainData.Humidity,
                Sunrise = dto.SysData.Sunrise,
                Sunset = dto.SysData.Sunset
            };

            return Result.Ok(weatherInfo);
        }
    }
}
