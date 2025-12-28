using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class PersonWallet
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public Guid WalletId { get; set; }
    public Person Person { get; set; } = new(); 
    public Wallet Wallet { get; set; } = new();
}
