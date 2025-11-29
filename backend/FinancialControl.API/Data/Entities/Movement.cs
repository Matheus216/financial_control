using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class Movement
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
    
}
