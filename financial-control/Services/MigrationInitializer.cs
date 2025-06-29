using Microsoft.EntityFrameworkCore.Infrastructure;
using financial_control.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace financial_control.Services;

public static class MigrationInitializer
{
    public static void InitializeMigration(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider.GetService<DbFinancialContext>();
        service?.Database.Migrate();
    }   
}