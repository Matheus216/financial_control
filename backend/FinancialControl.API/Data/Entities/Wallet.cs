using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class Wallet
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool Status { get; set; }
}
