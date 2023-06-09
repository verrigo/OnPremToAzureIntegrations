using Azure.Identity;
using AzureAppConfigurationIntegration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var appConfigurationConnectionString = builder.Configuration.GetConnectionString("AzureAppConfiguration");
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(appConfigurationConnectionString)
    .ConfigureKeyVault(kv =>
    {
        kv.SetCredential(new DefaultAzureCredential(
            new DefaultAzureCredentialOptions {}
            ));
    });
});
builder.Services.Configure<TestConfiguration>(builder.Configuration.GetSection("AzureAppConfigurationIntegration:TestConfiguration"));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/TestConfiguration", (IOptions<TestConfiguration> options) =>
{
    return options.Value;
}).WithName("GetTestConfiguration")
    .WithOpenApi();
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
