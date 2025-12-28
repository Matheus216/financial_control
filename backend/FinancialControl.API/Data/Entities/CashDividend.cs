using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public class CashDividend
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? AssetIssued { get; set; }
    public DateTime PaymentDate { get; set; }
    public double Rate { get; set; }
    public string? RelatedTo { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public string? IsinCode { get; set; }
    public string? Label { get; set; }
    public DateTime? LastDatePrior { get; set; }
    public string? Remarks { get; set; }
    
    [JsonIgnore]
    public DividendsData? DividendsData { get; set; }
    public Guid DividendsDataId { get; set; }
}
