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
                         select "Category", extract(year from "Timestamp") as "Year", extract(month from "Timestamp") as "Month", sum("Transactions"."Amount") as "Total" from "Transactions"
                         where "Transactions"."Category" is not null
                         group by "Category", extract(year from "Timestamp"), extract(month from "Timestamp")
                         """)
            .HasNoKey();
    }
}