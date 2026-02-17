using System;
using FinancialControl.API.Domain.Enums;

namespace FinancialControl.API.Contracts.Asset;

public class AssetCreateRequest()
{
    public string Description { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public AssetType Type { get; set; }
    public string? Currency { get; set; }
    public string? ShortName { get; set; }
    public string? LongName { get; set; }
    public double RegularMarketPrice { get; set; }
    public double MarketCap { get; set; }
    public string? LogoUrl { get; set; }
}
