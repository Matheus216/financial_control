using FinancialControl.API.Data;
using FinancialControl.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.API.Endpoints;

public static class PeopleEndpoints
{
    public static void MapPeopleEndpoints(this WebApplication app)
    {
        app.MapGet("/people:list", async (ApiDbContext context, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            var response = await context
                .People
                .AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }).WithTags("People"); 
        
        app.MapGet("/people", async (ApiDbContext context) =>
            await context.People.ToListAsync()).WithTags("People");

        app.MapGet("/people/{id}", async (ApiDbContext context, Guid id) =>
            await context.People.FindAsync(id) is { } People
                ? Results.Ok(People)
                : Results.NotFound()).WithTags("People");

        app.MapPost("/people", async (ApiDbContext context, People People) =>
        {
            context.People.Add(People);
            await context.SaveChangesAsync();
            return Results.Created($"/people/{People.Id}", People);
        }).WithTags("People");

        app.MapPut("/people/{id}", async (ApiDbContext context, Guid id, People inputPeople) =>
        {
            var People = await context.People.FindAsync(id);

            if (People is null) return Results.NotFound();

            People.Name = inputPeople.Name;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("People");

        app.MapDelete("/people/{id}", async (ApiDbContext context, Guid id) =>
        {
            if (await context.People.FindAsync(id) is { } People)
            {
                context.People.Remove(People);
                await context.SaveChangesAsync();
                return Results.Ok(People);
            }

            return Results.NotFound();
        }).WithTags("People");
    }
}
