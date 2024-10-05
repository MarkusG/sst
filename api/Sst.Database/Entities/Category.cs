using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sst.Database.Entities;

public class Category
{
    public int Id { get; set; }

    public required string Name { get; set; }
    
    public required int Position { get; set; }

    public required int? SuperCategoryId { get; set; }
    
    public Category? SuperCategory { get; set; }

    public List<Category> Subcategories { get; set; } = [];

    public List<Transaction> Transactions { get; set; } = [];
}

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasOne(c => c.SuperCategory)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(c => c.SuperCategoryId);

        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}