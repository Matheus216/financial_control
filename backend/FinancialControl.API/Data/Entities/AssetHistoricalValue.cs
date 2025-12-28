using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class AssetHistoricalValue
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public int Date { get; set; }
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }
    public int Volume { get; set; }
    public double AdjustedClose { get; set; }
    
    [JsonIgnore]
    public Asset? Asset { get; set; }
    public Guid AssetId { get; set; }
}