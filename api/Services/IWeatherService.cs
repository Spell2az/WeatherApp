using api.Models;
using FluentResults;

namespace api.Services
{
    public interface IWeatherService
    {
        Task<Result<WeatherInfo>> GetWeatherInfo(string location);
    }
}
