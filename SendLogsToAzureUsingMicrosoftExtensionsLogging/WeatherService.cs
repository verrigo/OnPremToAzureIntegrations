namespace SendLogsToAzureUsingMicrosoftExtensionsLogging
{
    internal class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly string[] _summaries = new[]{
                                        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                                    };
        public WeatherService(ILogger<WeatherService> logger)
        {
            _logger = logger;
        }

        WeatherForecast[] IWeatherService.GenerateRandomWeather()
        {
            _logger.LogInformation("Hmmm");
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                ))
                .ToArray();
            try
            {
                _logger.LogError(new Exception("This is it"), "This is ids");
               
                throw new Exception("Throwing");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
                return forecast;
        }

    }
}
