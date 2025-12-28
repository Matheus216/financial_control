using FinancialControl.API.Application.Providers.BRAPI;
using FinancialControl.API.Data;
using FinancialControl.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.API.Endpoints;

public static class AssetEndpoints
{
    public static RouteGroupBuilder MapAssetEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/assets:list", async (ApiDbContext context, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            var response = await context
                .Assets
                .AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }).WithTags("Asset"); 

        app.MapGet("/assets", async (ApiDbContext context) =>
            await context.Assets.ToListAsync()).WithTags("Asset");

        app.MapGet("/assets/{id}", async (ApiDbContext context, Guid id) =>
            await context.Assets.FindAsync(id) is { } asset
                ? Results.Ok(asset)
                : Results.NotFound()).WithTags("Asset");

        app.MapPost("/assets", async (ApiDbContext context, Asset asset) =>
        {
            context.Assets.Add(asset);
            await context.SaveChangesAsync();
            return Results.Created($"/assets/{asset.Id}", asset);
        }).WithTags("Asset");

        app.MapPut("/assets/{id}", async (ApiDbContext context, Guid id, Asset inputAsset) =>
        {
            var asset = await context.Assets.FindAsync(id);

            if (asset is null) return Results.NotFound();

            asset.Description = inputAsset.Description;
            asset.Ticker = inputAsset.Ticker;
            asset.Type = inputAsset.Type;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Asset");

        app.MapDelete("/assets/{id}", async (ApiDbContext context, Guid id) =>
        {
            if (await context.Assets.FindAsync(id) is { } asset)
            {
                context.Assets.Remove(asset);
                await context.SaveChangesAsync();
                return Results.Ok(asset);
            }

            return Results.NotFound();
        }).WithTags("Asset");

        app.MapGet("/pull-actual-informations/{ticker}", 
            async (ApiDbContext context, [FromServices] IBRAPI brapi, [FromRoute] string ticker) =>
            {
                var response = await brapi.GetCompactAssetByTicketAsync(ticker); 

                if (response is null)
                {
                    return Results.NotFound(); 
                }

                context.Assets.Add(response); 
                await context.SaveChangesAsync();

                return Results.Ok("Ticket added as success");

            }).WithTags("Asset");
        
        return app;
    }
}
