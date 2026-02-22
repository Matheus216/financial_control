namespace FinancialControl.API.Contracts.Summary.Request;

public class GetSummaryRequest
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
