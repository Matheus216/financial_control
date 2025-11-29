using FinancialControl.API.Data;
using FinancialControl.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.API.Endpoints;

public static class WalletEndpoints
{
    public static void MapWalletEndpoints(this WebApplication app)
    {
       app.MapGet("/wallet:list", async (ApiDbContext context, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            var response = await context
                .Wallets
                .AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }).WithTags("People"); 

        app.MapGet("/wallets", async (ApiDbContext context) =>
            await context.Wallets.ToListAsync()).WithTags("Wallet");

        app.MapGet("/wallets/{id}", async (ApiDbContext context, Guid id) =>
            await context.Wallets.FindAsync(id) is { } wallet
                ? Results.Ok(wallet)
                : Results.NotFound()).WithTags("Wallet");

        app.MapPost("/wallets", async (ApiDbContext context, Wallet wallet) =>
        {
            context.Wallets.Add(wallet);
            await context.SaveChangesAsync();
            return Results.Created($"/wallets/{wallet.Id}", wallet);
        }).WithTags("Wallet");

        app.MapPut("/wallets/{id}", async (ApiDbContext context, Guid id, Wallet inputWallet) =>
        {
            var wallet = await context.Wallets.FindAsync(id);

            if (wallet is null) return Results.NotFound();

            wallet.Description = inputWallet.Description;
            wallet.Status = inputWallet.Status;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Wallet");

        app.MapDelete("/wallets/{id}", async (ApiDbContext context, Guid id) =>
        {
            if (await context.Wallets.FindAsync(id) is { } wallet)
            {
                context.Wallets.Remove(wallet);
                await context.SaveChangesAsync();
                return Results.Ok(wallet);
            }

            return Results.NotFound();
        }).WithTags("Wallet");
    }
}
