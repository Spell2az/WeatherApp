using api.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{

    private readonly ILogger<WeatherController> logger;
    private readonly IWeatherService weatherService;

    public WeatherController(ILogger<WeatherController> logger, IWeatherService weatherService)
    {
        this.logger = logger;
        this.weatherService = weatherService;
    }

    [HttpPost]
    [Route("/get-weather")]
    public async Task<IActionResult> Post([FromForm] string? location)
    {
        if (string.IsNullOrEmpty(location))
        {
            return StatusCode(400);
        }
        var weatherInfo =  await weatherService.GetWeatherInfo(location);

        if (weatherInfo.IsFailed)
        {
            return StatusCode(500);
        }

        return new JsonResult(weatherInfo.Value);
    }
}
