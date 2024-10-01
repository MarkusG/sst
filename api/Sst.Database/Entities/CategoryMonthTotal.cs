using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sst.Database.Entities;

public record CategoryMonthTotal(string Category, int Year, int Month, decimal Total);

public class CategoryMonthTotalEntityTypeConfiguration : IEntityTypeConfiguration<CategoryMonthTotal>
{
    public void Configure(EntityTypeBuilder<CategoryMonthTotal> builder)
    {
        builder
            .ToSqlQuery($"""
                            select c."Name" as "Category", extract(year from "Timestamp") as "Year", extract(month from "Timestamp") as "Month", sum(t."Amount") as "Total"
                            from "Transactions" t
                                     inner join "Categories" as c on c."Id" = t."CategoryId"
                            where t."CategoryId" is not null
                            group by c."Name", extract(year from "Timestamp"), extract(month from "Timestamp")
                         """)
            .HasNoKey();
    }
}