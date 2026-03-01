using FinancialControl.API.Application.Interfaces;
using FinancialControl.API.Data;
using FinancialControl.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.API.Endpoints;

public class PersonEndpoints : IEndpointBase
{
    public void Map(IEndpointRouteBuilder app)
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

        app.MapPost("/persons", async (IKeycloakService service, ApiDbContext context, PersonCreateRequest request) =>
        {
            var createdUser = await service.CreateUserAsync(request); 

            var personEntity = (Person)request;

            personEntity.SetId(createdUser);  

            context.Persons.Add(personEntity);
            
            await context.SaveChangesAsync();
            
            return Results.Created($"/persons/{personEntity.Id}", personEntity);

        }).WithTags("Person");

        app.MapPut("/persons/{id}", async (IKeycloakService keycloak, ApiDbContext context, Guid id, PersonUpdateRequest request) =>
        {
            var Person = await context.Persons.FindAsync(id);

            if (Person is null) return Results.NotFound();

            await keycloak.UpdateUserAsync(id, request);

            context.Persons.Update((Person)request);  

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

    }
}
