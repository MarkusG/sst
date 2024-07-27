using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sst.Database.Entities;

namespace Sst.Database;

public class SstDbContext(DbContextOptions<SstDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<Item> Items { get; set; }

    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SstDbContext).Assembly);

        // to make DateTimeOffsets work with SQLite
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            foreach (var t in modelBuilder.Model.GetEntityTypes())
            {
                var props = t.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset)
                                || p.PropertyType == typeof(DateTimeOffset?));

                foreach (var p in props)
                {
                    modelBuilder.Entity(t.Name)
                        .Property(p.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }
    }
}