namespace FinancialControl.API.Configuration.HttpIntegration;

public abstract class HttpIntegrationConfiguration
{
    public string BaseUrl { get; set; } = string.Empty; 
    public string Token { get; set; } = string.Empty;
}
