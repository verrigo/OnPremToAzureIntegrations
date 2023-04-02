namespace SendLogsToAzureUsingMicrosoftExtensionsLogging
{
    internal interface IWeatherService
    {
        internal WeatherForecast[] GenerateRandomWeather();
    }
}
