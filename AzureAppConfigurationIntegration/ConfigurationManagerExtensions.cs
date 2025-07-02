public static class ConfigurationManagerExtensions
{
    public static void UseMediationLayerAppConfiguration(this ConfigurationManager configurationManager,
        Azure.Core.TokenCredential? credential = null)
    {
        credential = credential ?? new ManagedIdentityCredential();
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment is null)
            throw new InvalidOperationException("ASPNETCORE_ENVIRONMENT environment not set");
        configurationManager.AddAzureAppConfiguration(options =>
        {
            Uri endpoint = GetAzureAppConfigurationUrl(environment) ??
                           throw new InvalidOperationException(
                               "The environment variable 'AZURE_APPCONFIG_ENDPOINT' is not set or is empty.");
            options.Connect(endpoint, credential);
        });
    }

    private static Uri GetAzureAppConfigurationUrl(string environment)
    {
        throw new NotImplementedException();
    }

    public static void AddMediationLayerEventsConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AzureServiceBusConfiguration>(options => configuration.GetSection("AzureServiceBusConfiguration").Bind(options));
    }
}
