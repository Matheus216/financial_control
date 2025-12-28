using FinancialControl.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialControl.API.Data;
using Microsoft.AspNetCore.Mvc;
using FinancialControl.API.Endpoints.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FinancialControl.API.Endpoints;

public static class WalletEndpoints
{
    private const string TAG = "Wallet"; 

    public static RouteGroupBuilder MapWalletEndpoints(this RouteGroupBuilder app)
    {
       app.MapGet("/wallets:list", async (ApiDbContext context, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            var response = await context
                .Wallets
                .AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }).WithTags(TAG); 

        app.MapGet("/wallets", async (ApiDbContext context) =>
            await context.Wallets.ToListAsync())
            .WithTags(TAG);

        app.MapGet("/wallets/{id}", async (ApiDbContext context, Guid id) =>
            await context.Wallets.FindAsync(id) is { } wallet
                ? Results.Ok(wallet)
                : Results.NotFound())
            .WithTags(TAG);

        app.MapPost("/wallets", async (ApiDbContext context, [FromBody] Wallet wallet) =>
        {
            context.Wallets.Add(wallet);
            await context.SaveChangesAsync();
            return Results.Created($"/wallets/{wallet.Id}", wallet);
        }).WithTags(TAG);

        app.MapPut("/wallets/{id}", async (ApiDbContext context, Guid id, [FromBody] Wallet inputWallet) =>
        {
            var wallet = await context.Wallets.FindAsync(id);

            if (wallet is null) return Results.NotFound();

            wallet.Description = inputWallet.Description;
            wallet.Status = inputWallet.Status;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags(TAG);

        app.MapDelete("/wallets/{id}", async (ApiDbContext context, Guid id) =>
        {
            if (await context.Wallets.FindAsync(id) is { } wallet)
            {
                context.Wallets.Remove(wallet);
                await context.SaveChangesAsync();
                return Results.Ok(wallet);
            }

            return Results.NotFound();
        }).WithTags(TAG);


        app.MapPost("/wallets/link-person/{wallet}/{person}", LinkWalletToPersonAsync)
            .WithTags(TAG);

        app.MapPost("/wallets/add-asset", AddAssetAsync)
            .WithTags(TAG);

        return app; 
    }

    public static async Task<IResult> LinkWalletToPersonAsync(
        [FromServices] ApiDbContext dbContext, 
        [FromRoute] Guid wallet, 
        [FromRoute] Guid person, 
        CancellationToken cancellation)
    {
        var taskWallet = dbContext.Wallets.Where(x => x.Id == wallet).FirstOrDefaultAsync(cancellation); 
        var taskPerson = dbContext.Persons.Where(x => x.Id == person).FirstOrDefaultAsync(cancellation); 

        await Task.WhenAll(taskWallet, taskPerson);  

        if (taskWallet.Result is null || taskPerson.Result is null)
        {
            return Results.BadRequest("Wallet or Person invalid"); 
        }

        var personWallet = await dbContext.PersonWallets
            .Where(x => x.Id == wallet && x.PersonId == person)
            .FirstOrDefaultAsync(cancellation); 

        if (personWallet is not null)
        {
            return Results.BadRequest("This link existed in the database"); 
        }

        dbContext.PersonWallets.Add(new() { PersonId = person, WalletId = wallet });
        await dbContext.SaveChangesAsync(cancellation); 

        return Results.Created(); 
    }

    public static async Task<IResult> AddAssetAsync(
        [FromServices] ApiDbContext context, 
        [FromBody] WalletAssetCreateViewModel request,
        CancellationToken cancellationToken
        )
    {
        var taskWallet = context
            .Wallets
            .Include(x => x.WalletAsset)
            .FirstOrDefaultAsync(x => x.Id == request.Wallet, cancellationToken: cancellationToken);

        var taskAsset = context.Wallets.FirstOrDefaultAsync(x => x.Id == request.Asset, cancellationToken: cancellationToken);

        await Task.WhenAll(taskWallet, taskAsset); 

        if (taskWallet.Result is null || taskAsset.Result is null)
        {
            return Results.BadRequest("Asset or Wallet invalid"); 
        }

        if (taskWallet.Result.WalletAsset.Any(x => x.Asset.Id == request.Asset))
        {
            return Results.BadRequest("This asset exist in this table"); 
        }

        context.WalletAssets.Add(new() { 
            AssetId = request.Asset, 
            WalletId = request.Wallet, 
            Percentage = request.Percentage 
        }); 

        await context.SaveChangesAsync(cancellationToken); 

        return Results.Created(); 
    }

}
