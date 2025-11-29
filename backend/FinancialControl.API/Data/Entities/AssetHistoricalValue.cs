using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

/// <summary>
/// Create to make control of the historical price 
/// </summary>
public class AssetHistoricalPrice
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public decimal Min { get; set; }
    public decimal Max { get; set; }
    public DateTime Date { get; set; }
}
