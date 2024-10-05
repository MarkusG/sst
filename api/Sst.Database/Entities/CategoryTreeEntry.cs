using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sst.Database.Entities;

public record CategoryTreeEntry(int Id, string Name, int Level, int Position, int? SuperCategoryId, string Path);

public class CategoryTreeEntryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryTreeEntry>
{
    public void Configure(EntityTypeBuilder<CategoryTreeEntry> builder)
    {
        builder
            .ToSqlQuery($"""
                            select * from (with recursive categories as (
                                select "Id", "Name", 0 as "Level", "Position", "SuperCategoryId", cast("Id" as text) as "Path"
                                from "Categories"
                                where "SuperCategoryId" is null
                            
                                union all
                            
                                select c."Id", c."Name", "Level" + 1, c."Position", c."SuperCategoryId", cast("Path" || '.' || cast(c."Id" as text) as text) as Path
                                from "Categories" c
                                         inner join categories cats on cats."Id" = c."SuperCategoryId"
                            )
                            select * from categories order by "Path", "Position") as CategoryTreeEntries;
                         """);
    }
}