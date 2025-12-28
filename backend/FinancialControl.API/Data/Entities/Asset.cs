using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

/// <summary>
/// REIT - Real Estate Investment Trust
/// </summary>
public enum AssetType
{
    Stock,
    REIT, 
    Cripto,
    CDB, 
    SELIC
}

public class Asset
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public AssetType Type { get; set; }
    public string? Currency { get; set; }
    public string? ShortName { get; set; }
    public string? LongName { get; set; }
    public double RegularMarketPrice { get; set; }
    public double MarketCap { get; set; }
    public string? LogoUrl { get; set; }
    
    public ICollection<AssetHistoricalValue> HistoricalData { get; set; } = new List<AssetHistoricalValue>();
    public DividendsData? DividendsData { get; set; }
}
