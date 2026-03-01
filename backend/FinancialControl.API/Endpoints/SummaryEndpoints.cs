using FinancialControl.API.Contracts.Summary.Request;
using FinancialControl.API.Data;

namespace FinancialControl.API.Endpoints;

public class SummaryEndpoints : IEndpointBase
{
    private const string TAG = "Summary";

    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (ApiDbContext context, [AsParameters] GetSummaryRequest request) =>
        {

        }).WithTags(TAG);

    }
}
