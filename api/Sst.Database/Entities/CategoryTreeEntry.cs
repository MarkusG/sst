using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sst.Database.Entities;

public record CategoryTreeEntry(int Id, string Name, int Level, int Position, int? ParentId, int[] Path);

public class CategoryTreeEntryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryTreeEntry>
{
    public void Configure(EntityTypeBuilder<CategoryTreeEntry> builder)
    {
        builder
            .ToSqlQuery($"""
                            select * from (with recursive categories as (
                                select "Id", "Name", 0 as "Level", "Position", "ParentId", array["Position"] as "Path"
                                from "Categories"
                                where "ParentId" is null
                            
                                union all
                            
                                select c."Id", c."Name", "Level" + 1, c."Position", c."ParentId", array_append("Path", c."Position") as Path
                                from "Categories" c
                                         inner join categories cats on cats."Id" = c."ParentId"
                            )
                            select * from categories order by "Path", "Position") as CategoryTreeEntries;
                         """);
    }
}