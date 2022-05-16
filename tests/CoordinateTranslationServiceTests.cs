using System;
using System.IO;
using System.Net;
using Xunit;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using api.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;


namespace tests;

public class CoordinateTranslationServiceTests
{
    readonly IConfiguration configuration;

    public CoordinateTranslationServiceTests()
    {
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", false, false)
            .Build();
    }

    [Fact]
    public async Task CoordinateServiceReturnsCoordinates()
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
                Content = new StringContent("[{\"lat\":36.4761,\"lon\":-119.4432,\"country\":\"US\",\"state\":\"CA\"}]"),
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        client.BaseAddress = new Uri("http://api.openweathermap.org");
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

        var coordinateTranslatorService = new CoordinateTranslationService(mockFactory.Object, configuration);
        var result = await coordinateTranslatorService.GetCoordinatesFromLocation("location");

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(36.4761, result.Value.Latitude);
        Assert.Equal(-119.4432, result.Value.Longitude);
    }
}
