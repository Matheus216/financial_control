using financial_control_domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace financial_control.Infrastructure.Context;

public class DbFinancialContext : DbContext
{
    private readonly IConfiguration? _configuration;

    public DbFinancialContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_configuration is null 
            ?   "Host=postgres;Port=5432;Database=DbFinancial;Username=admin;Password=admin"
            :  _configuration.GetConnectionString("DbFinancial") );
    }

    public DbSet<FinancialModel> Financial { get; set; }
    public DbSet<PersonModel> Person { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialModel>().HasKey(f => f.Id);
        modelBuilder.Entity<PersonModel>().HasKey(p => p.Id);
    }
}