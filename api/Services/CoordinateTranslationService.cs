using System.Text.Json;
using api.Models;
using FluentResults;

namespace api.Services;

public class CoordinateTranslationService : ICoordinateTranslationService
{
    readonly IHttpClientFactory clientFactory;
    readonly IConfiguration configuration;

    public CoordinateTranslationService(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        this.clientFactory = clientFactory;
        this.configuration = configuration;

    }
    public async Task<Result<Coordinates>>  GetCoordinatesFromLocation(string location)
    {
        var httpClient = clientFactory.CreateClient("WeatherApiClient");
        var apiKey = this.configuration["ApiKey"];
        using (var response = await httpClient.GetAsync($"/geo/1.0/direct?q={location}&limit=1&appid={apiKey}"))
        {
            if (!response.IsSuccessStatusCode)
            {
                return Result.Fail("Failed to get coordinates");
            }

            var data = await response.Content.ReadAsStringAsync();

            Coordinates? coordinates;
            try
            {
                coordinates = JsonSerializer.Deserialize<Coordinates[]>(data)?.FirstOrDefault();
            }
            catch (Exception e)
            {
                //log it
                return Result.Fail("Failed to get coordinates");
            }

            if (coordinates == null)
            {
                return Result.Fail("Failed to get coordingates");
            }
            return Result.Ok(coordinates);
        }
    }
}
