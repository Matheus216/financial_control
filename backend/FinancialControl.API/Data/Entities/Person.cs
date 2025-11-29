using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;
public class People
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<Wallet> Wallets { get; set; } = [];
}
