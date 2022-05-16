using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using FluentResults;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;

namespace tests;

public class WeatherServiceTests
{
    readonly IConfiguration configuration;

    public WeatherServiceTests()
    {
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", false, false)
            .Build();
    }

    [Fact]
    public async Task WeatherServiceReturnsCorrectWeatherInfo()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"clear sky\",\"icon\":\"01d\"}],\"main\":{\"temp\":282.55,\"feels_like\":281.86,\"temp_min\":280.37,\"temp_max\":284.26,\"pressure\":1023,\"humidity\":100},\"sys\":{\"type\":1,\"id\":5122,\"message\":0.0139,\"country\":\"US\",\"sunrise\":1560343627,\"sunset\":1560396563},\"name\":\"Mountain View\"}")
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        client.BaseAddress = new Uri("http://api.openweathermap.org");
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

        var coordinateTranslatorService = new Mock<ICoordinateTranslationService>();
        coordinateTranslatorService
            .Setup(x => x.GetCoordinatesFromLocation(It.IsAny<string>()))
            .ReturnsAsync(Result.Ok(new Coordinates { Latitude = 0.00, Longitude = 0.00 }));

        var weatherService = new WeatherService(mockFactory.Object, coordinateTranslatorService.Object, configuration);
        var result = await weatherService.GetWeatherInfo("some location");

        var weatherInfo = new WeatherInfo
        {
            LocationName = "Mountain View",
            WeatherMain = "Clear",
            Description = "clear sky",
            Icon = "01d",
            CurrentTemp = 282.55f,
            MaximumTemp = 284.26f,
            MinimumTemp = 280.37f,
            Pressure = 1023,
            Humidity = 100,
            Sunrise = 1560343627,
            Sunset = 1560396563
        };

        Assert.True(result.IsSuccess);
        Assert.Equal(JsonSerializer.Serialize(weatherInfo), JsonSerializer.Serialize(result.Value));
    }
}
