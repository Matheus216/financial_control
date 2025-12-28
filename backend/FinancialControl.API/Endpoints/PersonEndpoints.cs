using FinancialControl.API.Data;
using FinancialControl.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.API.Endpoints;

public static class PersonEndpoints
{
    public static RouteGroupBuilder MapPersonEndpoints(this RouteGroupBuilder app)
    {
        app.MapGet("/persons:list", async (ApiDbContext context, [FromQuery] int page, [FromQuery] int pageSize) =>
        {
            var response = await context
                .Persons
                .AsNoTracking()
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }).WithTags("Person"); 
        
        app.MapGet("/persons", async (ApiDbContext context) =>
            await context.Persons.ToListAsync()).WithTags("Person");

        app.MapGet("/persons/{id}", async (ApiDbContext context, Guid id) =>
            await context.Persons.FindAsync(id) is { } Person
                ? Results.Ok(Person)
                : Results.NotFound()).WithTags("Person");

        app.MapPost("/persons", async (ApiDbContext context, Person Person) =>
        {
            context.Persons.Add(Person);
            await context.SaveChangesAsync();
            return Results.Created($"/persons/{Person.Id}", Person);
        }).WithTags("Person");

        app.MapPut("/persons/{id}", async (ApiDbContext context, Guid id, Person inputPerson) =>
        {
            var Person = await context.Persons.FindAsync(id);

            if (Person is null) return Results.NotFound();

            Person.Name = inputPerson.Name;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Person");

        app.MapDelete("/persons/{id}", async (ApiDbContext context, Guid id) =>
        {
            if (await context.Persons.FindAsync(id) is { } Person)
            {
                context.Persons.Remove(Person);
                await context.SaveChangesAsync();
                return Results.Ok(Person);
            }

            return Results.NotFound();
        }).WithTags("Person");

        return app;
    }
}
