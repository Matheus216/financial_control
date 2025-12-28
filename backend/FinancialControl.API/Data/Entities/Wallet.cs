using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class Wallet
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool Status { get; set; }

    public Collection<WalletAsset> WalletAsset { get; set; } = new(); 
}
