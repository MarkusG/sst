using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sst.Database.Entities;

public class Category
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required int Position { get; set; }

    public required int? ParentId { get; set; }

    public Category? ParentCategory { get; set; }

    public List<Category> Subcategories { get; set; } = [];
}

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(c => c.ParentId);

        builder.HasIndex(c => c.Name)
            .IsUnique();
    }
}