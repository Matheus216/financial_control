using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class DividendsData
{
    [JsonIgnore]
    public Guid Id { get; set; }
    
    public Guid AssetId { get; set; }
    [JsonIgnore]
    public Asset Asset { get; set; }
    
    public ICollection<CashDividend> CashDividends { get; set; } = new List<CashDividend>();
    public ICollection<StockDividend> StockDividends { get; set; } = new List<StockDividend>();
}
