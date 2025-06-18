using Microsoft.Extensions.Configuration;
using financial_control_domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace financial_control.Infrastructure.Context;

public class DbFinancialContext(
    IConfiguration configuration,
    ILogger<DbFinancialContext> logger
) : DbContext
{
    private readonly IConfiguration? _configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder options )
    {
        ArgumentNullException.ThrowIfNull(_configuration);
        logger.LogInformation("Building migration... conection string: {C}", _configuration.GetConnectionString("DbFinancial"));
        options
            .UseNpgsql(_configuration.GetConnectionString("DbFinancial"))
            .UseLowerCaseNamingConvention();
    }

    public DbSet<FinancialModel> Financial { get; set; }
    public DbSet<PersonModel> Person { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialModel>().HasKey(f => f.Id);
        modelBuilder.Entity<PersonModel>().HasKey(p => p.Id);
    }
}