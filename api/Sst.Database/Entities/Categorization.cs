using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sst.Database.Entities;

public class Categorization
{
    public int Id { get; set; }
    
    public required int TransactionId { get; set; }
    
    public Transaction? Transaction { get; set; }
    
    public required decimal Amount { get; set; }
    
    public required int CategoryId { get; set; }
    
    public Category? Category { get; set; }
    
    public required int Position { get; set; }
    
    public string? Description { get; set; }
}

public class CategorizationEntityTypeConfiguration : IEntityTypeConfiguration<Categorization>
{
    public void Configure(EntityTypeBuilder<Categorization> builder)
    {
        builder.Property(c => c.Amount)
            .HasPrecision(10, 2);

        builder.Property(c => c.Description)
            .HasMaxLength(100);
    }
}