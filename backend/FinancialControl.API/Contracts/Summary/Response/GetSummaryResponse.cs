using System;

namespace FinancialControl.API.Contracts.Summary.Response;

public class GetSummaryResponse
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal TotalRemaining { get; set; }
}
