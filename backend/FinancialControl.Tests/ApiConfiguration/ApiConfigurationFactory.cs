using FinancialControl.API;
using FinancialControl.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using TUnit.AspNetCore;
using TUnit.Core.Interfaces;

namespace FinancialControl.Tests.ApiConfiguration;

public class IMemoryDatabase : IAsyncInitializer, IAsyncDisposable
{
    public PostgreSqlContainer Container { get; } = new PostgreSqlBuilder("postgres:16-alpine")
        .Build();

    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        await ConfigureMigration();
    }

    private static async Task ConfigureMigration()
    {
        var options = new DbContextOptionsBuilder<ApiDbContext>()
               .UseNpgsql("Server=localhost;Port=5432;Database=financial_control;User Id=user;Password=password;")
               .Options;

        using var db = new ApiDbContext(options);
        await db.Database.MigrateAsync();
    }
}

public class WebApplicationFactory : TestWebApplicationFactory<IApplicationMarker>
{
    [ClassDataSource<IMemoryDatabase>(Shared = SharedType.PerTestSession)]
    public IMemoryDatabase Database { get; init; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", "Server=localhost;Port=5432;Database=financial_control;User Id=user;Password=password;" },
            });
        });
    }
}

public abstract class TestBase : WebApplicationTest<WebApplicationFactory, IApplicationMarker>
{
}
