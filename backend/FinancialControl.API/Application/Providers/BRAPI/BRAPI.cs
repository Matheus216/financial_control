using FinancialControl.API.Data.Entities;
using FinancialControl.API.ViewModel;

namespace FinancialControl.API.Application.Providers.BRAPI;

public class BRAPI(IHttpClientFactory httpFactory) : IBRAPI
{   
    private readonly HttpClient httpClient = httpFactory.CreateClient("BRAPI");

    public async Task<Asset?> GetCompactAssetByTicketAsync(string ticker)
    {
        var response = await httpClient.GetAsync($"/api/quote/{ticker}"); 

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadFromJsonAsync<ResponseAPI<Asset>>();
            return responseContent?.Results?.FirstOrDefault() ?? throw new NullReferenceException("Error to integration");
        }

        return null;
    }
}
