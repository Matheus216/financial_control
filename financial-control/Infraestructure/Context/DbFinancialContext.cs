using financial_control.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace financial_control.Infraestructure.Context;

public class DbFinancialContext : DbContext
{
    public DbFinancialContext(DbContextOptionsBuilder options, IConfiguration _configuration) 
    {
        options.UseNpgsql(_configuration.GetConnectionString("DbFinancial"));
    }

    public DbSet<FinancialModel> Financial { get; set; }
    public DbSet<PersonModel> Person { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialModel>().HasKey(f => f.Id);
        modelBuilder.Entity<PersonModel>().HasKey(p => p.Id);
    }
}