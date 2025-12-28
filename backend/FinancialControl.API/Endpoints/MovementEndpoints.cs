using FinancialControl.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialControl.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Endpoints;

public static class MovementEndpoints
{
    private const string TAG = "Movement";
    public static RouteGroupBuilder MapMovementEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/movements:list", async (ApiDbContext context, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            var response = await context
                .Movements
                .AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }).WithTags(TAG);

        app.MapGet("/movements", async (ApiDbContext context) =>
            await context.Movements.ToListAsync()).WithTags(TAG);

        app.MapGet("/movements/{id}", async (ApiDbContext context, Guid id) =>
            await context.Movements.FindAsync(id) is { } movements
                ? Results.Ok(movements)
                : Results.NotFound()).WithTags(TAG);

        app.MapPost("/movements", async (ApiDbContext context, Movement movements) =>
        {
            context.Movements.Add(movements);
            await context.SaveChangesAsync();
            return Results.Created($"/movements/{movements.Id}", movements);
        }).WithTags(TAG);

        app.MapPut("/movements/{id}", async (ApiDbContext context, Guid id, Movement inputMovements) =>
        {
            var movements = await context.Movements.FindAsync(id);

            if (movements is null) return Results.NotFound();

            movements.Description = inputMovements.Description;
            movements.Value = inputMovements.Value;
            movements.Date = inputMovements.Date;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags(TAG);

        app.MapDelete("/movements/{id}", async (ApiDbContext context, Guid id) =>
        {
            if (await context.Movements.FindAsync(id) is { } movements)
            {
                context.Movements.Remove(movements);
                await context.SaveChangesAsync();
                return Results.Ok(movements);
            }

            return Results.NotFound();
        }).WithTags(TAG);

        return app; 
    }
}
