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
        return service; 
    }

    public static IServiceCollection ConfigureKeys(this IServiceCollection service, IConfiguration config)
    {
        BRAPIConfiguration brbapi = new(); 
        AlphaVantageConfiguration alpha = new();
        
        config.Bind(brbapi);
        config.Bind(alpha);

        service.AddSingleton<BRAPIConfiguration>(brbapi); 
        service.AddSingleton<AlphaVantageConfiguration>(alpha); 

        return service; 
    }
}
