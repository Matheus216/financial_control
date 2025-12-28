using FinancialControl.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialControl.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Endpoints;

public static class RevenueEndpoints
{
    private const string TAG = "Revenue";
    public static RouteGroupBuilder MapRevenueEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/revenue:list", async (ApiDbContext context, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            var response = await context
                .Revenues
                .AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }).WithTags(TAG);

        app.MapGet("/revenue", async (ApiDbContext context) =>
            await context.Revenues.ToListAsync()).WithTags(TAG);

        app.MapGet("/revenue/{id}", async (ApiDbContext context, Guid id) =>
            await context.Revenues.FindAsync(id) is { } revenue
                ? Results.Ok(revenue)
                : Results.NotFound()).WithTags(TAG);

        app.MapPost("/revenue", async (ApiDbContext context, Revenue revenue) =>
        {
            context.Revenues.Add(revenue);
            await context.SaveChangesAsync();
            return Results.Created($"/revenue/{revenue.Id}", revenue);
        }).WithTags(TAG);

        app.MapPut("/revenue/{id}", async (ApiDbContext context, Guid id, Revenue inputrevenue) =>
        {
            var revenue = await context.Revenues.FindAsync(id);

            if (revenue is null) return Results.NotFound();

            revenue.Description = inputrevenue.Description;
            revenue.Value = inputrevenue.Value;
            revenue.Date = inputrevenue.Date;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags(TAG);

        app.MapDelete("/revenue/{id}", async (ApiDbContext context, Guid id) =>
        {
            if (await context.Revenues.FindAsync(id) is { } revenue)
            {
                context.Revenues.Remove(revenue);
                await context.SaveChangesAsync();
                return Results.Ok(revenue);
            }

            return Results.NotFound();
        }).WithTags(TAG);

        return app;
    }
}
