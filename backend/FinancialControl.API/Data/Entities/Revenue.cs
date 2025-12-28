using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class Revenue
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public bool IsRecurrent { get; set; }
}
