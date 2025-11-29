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
    public string Code { get; set; } = string.Empty;
    public AssetType Type { get; set; }
}
