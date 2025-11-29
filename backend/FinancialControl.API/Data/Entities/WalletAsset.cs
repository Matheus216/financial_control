
using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;
public class WalletAsset
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public Guid AssetId { get; set; }
    public Wallet Wallet { get; set; } = new();
    public Asset Asset { get; set; } = new();
    public decimal Percentage { get; set; }
}
