using financial_control.Infraestructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace financial_control.Infraestructure.Context;

public class DbFinancialContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbFinancialContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Host=postgres;Port=5432;Database=DbFinancial;Username=admin;Password=admin");
    }

    public DbSet<FinancialModel> Financial { get; set; }
    public DbSet<PersonModel> Person { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialModel>().HasKey(f => f.Id);
        modelBuilder.Entity<PersonModel>().HasKey(p => p.Id);
    }
}