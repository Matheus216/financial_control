using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class StockDividend
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? AssetIssued { get; set; }
    public int Factor { get; set; }
    public string? CompleteFactor { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public string? IsinCode { get; set; }
    public string? Label { get; set; }
    public DateTime? LastDatePrior { get; set; }
    public string? Remarks { get; set; }
    
    [JsonIgnore]
    public DividendsData? DividendsData { get; set; }
    public Guid DividendsDataId { get; set; }
}
