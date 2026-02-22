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
    public DbSet<Person> Persons { get; set; }
    public DbSet<PersonWallet> PersonWallets { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletAsset> WalletAssets { get; set; }
    public DbSet<Revenue> Revenues { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<AssetHistoricalValue> AssetHistoricalValues { get; set; }
    public DbSet<DividendsData> DividendsData { get; set; }
    public DbSet<CashDividend> CashDividends { get; set; }
    public DbSet<StockDividend> StockDividends { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonWallet>()
            .HasKey(pw => pw.Id);

        modelBuilder.Entity<PersonWallet>()
            .HasOne(pw => pw.Person)
            .WithMany()
            .HasForeignKey(pw => pw.PersonId);

        modelBuilder.Entity<PersonWallet>()
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
            .HasOne(o => o.PersonWallet)
            .WithMany()
            .HasForeignKey(o => o.PersonWalletId);

        modelBuilder.Entity<AssetHistoricalValue>()
            .HasOne(o => o.Asset)
            .WithMany(p => p.HistoricalData)
            .HasForeignKey(o => o.AssetId);

        modelBuilder.Entity<Asset>()
            .HasOne(a => a.DividendsData)
            .WithOne(d => d.Asset)
            .HasForeignKey<DividendsData>(d => d.AssetId);

        modelBuilder.Entity<Person>()
            .HasMany(x => x.Transactions)
            .WithOne(x => x.Person)
            .HasForeignKey(d => d.PersonId); 

        modelBuilder.Entity<CashDividend>()
            .HasOne(cd => cd.DividendsData)
            .WithMany(dd => dd.CashDividends)
            .HasForeignKey(cd => cd.DividendsDataId);

        modelBuilder.Entity<StockDividend>()
            .HasOne(sd => sd.DividendsData)
            .WithMany(dd => dd.StockDividends)
            .HasForeignKey(sd => sd.DividendsDataId);

        foreach (var model in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder
                .Entity(model.ClrType)
                .ToTable(model.ClrType.Name);
        }
    }
}
