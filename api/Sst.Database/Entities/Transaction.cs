using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sst.Database.Entities;

public class Transaction
{
    public int Id { get; set; }

    public required string PlaidId { get; set; }

    public required string AccountName { get; set; }

    public string? AccountMask { get; set; }

    public required decimal Amount { get; set; }

    public required string Currency { get; set; }

    public int? CategoryId { get; set; }
    
    public Category? Category { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public required string Description { get; set; }
}

public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasIndex(t => t.PlaidId)
            .IsUnique();

        builder.Property(t => t.Amount)
            .HasPrecision(10, 2);
    }
}