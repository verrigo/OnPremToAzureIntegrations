using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Logging.ApplicationInsights;
using SendLogsToAzureUsingMicrosoftExtensionsLogging;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Logging.AddApplicationInsights(
    configureTelemetryConfiguration: (config)=>
    config.ConnectionString = builder.Configuration.GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"),
    configureApplicationInsightsLoggerOptions: (options) => { }
    );
//This is in case I want to log only specific namespaces (what-is-this-category-thing = namespace). Minimum log level will be LogLevel.Information
//builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("SendLogsToAzureUsingMicrosoftExtensionsLogging", LogLevel.Information);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", ([FromServices]IWeatherService weatherService) =>
{
    return weatherService.GenerateRandomWeather();
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
