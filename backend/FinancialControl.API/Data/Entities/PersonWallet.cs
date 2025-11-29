using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class PeopleWallet
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public Guid PeopleId { get; set; }
    public Guid WalletId { get; set; }
    public People People { get; set; } = new(); 
    public Wallet Wallet { get; set; } = new();
}
