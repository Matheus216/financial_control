using FinancialControl.API.Data;
using FinancialControl.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.API.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("/orders:list", async (ApiDbContext context, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            var response = await context
                .Orders
                .AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }).WithTags("Order"); 

        app.MapGet("/orders", async (ApiDbContext context) =>
            await context.Orders.ToListAsync()).WithTags("Order");

        app.MapGet("/orders/{id}", async (ApiDbContext context, Guid id) =>
            await context.Orders.FindAsync(id) is { } order
                ? Results.Ok(order)
                : Results.NotFound()).WithTags("Order");

        app.MapPost("/orders", async (ApiDbContext context, Order order) =>
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return Results.Created($"/orders/{order.Id}", order);
        }).WithTags("Order");

        app.MapPut("/orders/{id}", async (ApiDbContext context, Guid id, Order inputOrder) =>
        {
            var order = await context.Orders.FindAsync(id);

            if (order is null) return Results.NotFound();

            order.PeopleWalletId = inputOrder.PeopleWalletId;
            order.Date = inputOrder.Date;
            order.Quantity = inputOrder.Quantity;
            order.Price = inputOrder.Price;
            order.Type = inputOrder.Type;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Order");

        app.MapDelete("/orders/{id}", async (ApiDbContext context, Guid id) =>
        {
            if (await context.Orders.FindAsync(id) is { } order)
            {
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return Results.Ok(order);
            }

            return Results.NotFound();
        }).WithTags("Order");
    }
}
