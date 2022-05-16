using api;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient("WeatherApiClient",client =>
{
    client.BaseAddress = new Uri("http://api.openweathermap.org");
});

builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICoordinateTranslationService, CoordinateTranslationService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => options.AllowAnyOrigin());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
