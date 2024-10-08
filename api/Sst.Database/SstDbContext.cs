using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Sst.Database.Entities;

namespace Sst.Database;

public class SstDbContext(DbContextOptions<SstDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<Item> Items { get; set; }

    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SstDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }
}