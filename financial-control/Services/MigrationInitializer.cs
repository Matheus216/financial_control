using financial_control.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace financial_control.Services;


public static class MigrationInitializer
{
    public static void InitializeMigration(this IApplicationBuilder builder)
    {
        using (var scope =  builder.ApplicationServices.CreateScope())
        {
            var service = scope.ServiceProvider.GetService<DbFinancialContext>();
            service?.Database.Migrate();
        }
    }   
}