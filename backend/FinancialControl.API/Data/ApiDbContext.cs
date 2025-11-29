using FinancialControl.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.API.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<Asset> Assets { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<People> People { get; set; }
    public DbSet<PeopleWallet> PeopleWallets { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletAsset> WalletAssets { get; set; }

    public DbSet<Revenue> Revenues { get; set; }
    public DbSet<Movement> Movements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PeopleWallet>()
            .HasKey(pw => pw.Id);

        modelBuilder.Entity<PeopleWallet>()
            .HasOne(pw => pw.People)
            .WithMany()
            .HasForeignKey(pw => pw.PeopleId);

        modelBuilder.Entity<PeopleWallet>()
            .HasOne(pw => pw.Wallet)
            .WithMany()
            .HasForeignKey(pw => pw.WalletId);

        modelBuilder.Entity<WalletAsset>()
            .HasKey(wa => wa.Id);

        modelBuilder.Entity<WalletAsset>()
            .HasOne(wa => wa.Wallet)
            .WithMany()
            .HasForeignKey(wa => wa.WalletId);

        modelBuilder.Entity<WalletAsset>()
            .HasOne(wa => wa.Asset)
            .WithMany()
            .HasForeignKey(wa => wa.AssetId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.PeopleWallet)
            .WithMany()
            .HasForeignKey(o => o.PeopleWalletId);
    }
}
