using FinancialControl.API.Data.Entities;

namespace FinancialControl.API.Application.Interfaces;

public interface IIntegrationProvider
{
    Task<Asset?> GetCompactAssetByTicketAsync(string ticker);
}
