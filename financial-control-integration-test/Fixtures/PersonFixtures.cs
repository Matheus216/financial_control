using financial_control_integration_test.Configuration;
using financial_control.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using financial_control_domain.Models;

namespace financial_control_integration_test.Fixtures;

public class PersonFixtures
{
    public static async Task CreatePerson(ApiFactory configuration, bool create) {
        using var scope = configuration.Services.CreateScope();

        var provider = scope.ServiceProvider;
        var context = provider.GetRequiredService<DbFinancialContext>();

        if (create) {
            await context.Person.AddAsync(new PersonModel (1,"Test1" ));
            await context.Person.AddAsync(new PersonModel (1,"Test2" ));
            await context.Person.AddAsync(new PersonModel (1,"Test3" ));
            await context.Person.AddAsync(new PersonModel (1,"Test4" ));
            await context.SaveChangesAsync();
        }
    }
}
