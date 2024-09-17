using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sst.Database.Entities;

public class Account
{
    public int Id { get; set; }
    
    public required string PlaidId { get; set; }
    
    public required string Name { get; set; }
    
    public required decimal? AvailableBalance { get; set; }
    
    public required decimal? CurrentBalance { get; set; }
    
    public int ItemId { get; set; }
    
    public Item? Item { get; set; }
}

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasIndex(t => t.PlaidId)
            .IsUnique();

        builder.Property(t => t.AvailableBalance)
            .HasPrecision(10, 2);
        
        builder.Property(t => t.CurrentBalance)
            .HasPrecision(10, 2);
    }
}