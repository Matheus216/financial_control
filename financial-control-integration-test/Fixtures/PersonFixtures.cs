using financial_control.Infrastructure.Context;
using financial_control_domain.Models;
using financial_control_integration_test.Configuration;
using financial_control.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace financial_control_integration_test.Fixtures;

public class PersonFixtures
{
    public static async Task CreatePerson(ApiFactory<IInitialProject> configuration, bool create) {
        using var scope = configuration.Services.CreateScope();

        var provider = scope.ServiceProvider;
        var context = provider.GetRequiredService<DbFinancialContext>();

        if (create) {
            await context.Person.AddAsync(new PersonModel { Name = "Test1" });
            await context.Person.AddAsync(new PersonModel { Name = "Test2" });
            await context.Person.AddAsync(new PersonModel { Name = "Test3" });
            await context.Person.AddAsync(new PersonModel { Name = "Test4" });
            await context.SaveChangesAsync();
        }
    }
}
