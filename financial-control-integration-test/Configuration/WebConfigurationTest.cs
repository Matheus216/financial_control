using financial_control.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace financial_control_integration_test.Configuration;

public class WebConfigurationTest<TProgram>
    : WebApplicationFactory<Program> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        var root = new InMemoryDatabaseRoot();
         
        builder.ConfigureServices(services => {
            services.RemoveAll(typeof(DbContextOptions<DbFinancialContext>));

            services.AddDbContext<DbFinancialContext>(options => {
                options.UseInMemoryDatabase("FinancialDb", root);
            });
        });

        builder.UseEnvironment("Testing");
    }
}
