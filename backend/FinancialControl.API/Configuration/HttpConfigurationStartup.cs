using FinancialControl.API.Application.Providers.BRAPI;
using FinancialControl.API.Configuration.HttpIntegration;

namespace FinancialControl.API.Configuration;

public static class HttpConfigurationStartup
{
    public static IServiceCollection ConfigureHttp(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddHttpClient("BRAPI", client =>
        {
            client.BaseAddress = new Uri(  configuration.GetSection("BRAPI")["BaseUrl"]!); 
            client.Timeout = TimeSpan.FromSeconds(100);

            client.DefaultRequestHeaders.Add("Authorization", configuration.GetSection("BRAPI")["Token"]!);
        });

        service.AddHttpClient("AlphaVantage", client =>
        {
            client.BaseAddress = new Uri(configuration.GetSection("AlphaVantage")["BaseUrl"]!); 
            client.Timeout = TimeSpan.FromSeconds(100);

            client.DefaultRequestHeaders.Add("Authorization", configuration.GetSection("BRAPI")["Token"]!);
        });

        return service;
    }

    public static IServiceCollection ConfigureDI(this IServiceCollection service)
    {
        service.AddScoped<IBRAPI, BRAPI>();
        service.AddScoped<FinancialControl.API.Application.Interfaces.IKeycloakService, FinancialControl.API.Application.Providers.Keycloak.KeycloakService>();
        return service;
    }

    public static IServiceCollection ConfigureKeys(this IServiceCollection service, IConfiguration config)
    {
        BRAPIConfiguration brbapi = new();
        AlphaVantageConfiguration alpha = new();
        KeycloakConfiguration keycloak = new();

        config.Bind("BRAPI", brbapi);
        config.Bind("AlphaVantage", alpha);
        config.Bind("Keycloak", keycloak);

        service.AddSingleton<BRAPIConfiguration>(brbapi);
        service.AddSingleton<AlphaVantageConfiguration>(alpha);
        service.AddSingleton<KeycloakConfiguration>(keycloak);

        return service;
    }
}
