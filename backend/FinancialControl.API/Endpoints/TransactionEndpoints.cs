using FinancialControl.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialControl.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Endpoints;

public static class MovementEndpoints
{
    private const string TAG = "Transaction";
    public static RouteGroupBuilder MapTransactionEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/transactions:list", async (ApiDbContext context, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            var response = await context
                .Transactions
                .AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }).WithTags(TAG);

        app.MapGet("/transactions", async (ApiDbContext context) =>
            await context.Transactions.ToListAsync()).WithTags(TAG);

        app.MapGet("/transactions/{id}", async (ApiDbContext context, Guid id) =>
            await context.Transactions.FindAsync(id) is { } transactions
                ? Results.Ok(transactions)
                : Results.NotFound()).WithTags(TAG);

        app.MapPost("/transactions", async (ApiDbContext context, Transaction transactions) =>
        {
            context.Transactions.Add(transactions);
            await context.SaveChangesAsync();
            return Results.Created($"/transactions/{transactions.Id}", transactions);
        }).WithTags(TAG);

        app.MapPut("/transactions/{id}", async (ApiDbContext context, Guid id, Transaction inputMovements) =>
        {
            var transactions = await context.Transactions.FindAsync(id);

            if (transactions is null) return Results.NotFound();

            transactions.Description = inputMovements.Description;
            transactions.Value = inputMovements.Value;
            transactions.Date = inputMovements.Date;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags(TAG);

        app.MapDelete("/transactions/{id}", async (ApiDbContext context, Guid id) =>
        {
            if (await context.Transactions.FindAsync(id) is { } transactions)
            {
                context.Transactions.Remove(transactions);
                await context.SaveChangesAsync();
                return Results.Ok(transactions);
            }

            return Results.NotFound();
        }).WithTags(TAG);

        return app; 
    }
}
