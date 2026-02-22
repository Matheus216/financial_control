using FinancialControl.API.Contracts.Summary.Request;
using FinancialControl.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.API.Endpoints;

public static class SummaryEndpoints
{
    private const string TAG = "Summary";

    public static RouteGroupBuilder MapSummaryEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("", async (ApiDbContext context, [AsParameters] GetSummaryRequest request) =>
        {

        }).WithTags(TAG);

        return app; 
    }
}
