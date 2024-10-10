using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sst.Database.Entities;

public record CashFlowTreeEntry(int Id, string Name, int Level, decimal TreeTotal, decimal CategoryTotal, int Year, int Month);

public class CategoryMonthTotalTreeEntryEntityTypeConfiguration : IEntityTypeConfiguration<CashFlowTreeEntry>
{
    public void Configure(EntityTypeBuilder<CashFlowTreeEntry> builder)
    {
        builder.ToSqlQuery(
            $"""
                with recursive
                    categories as (select "Id", "Name", 0 as "Level", "Position", "ParentId", array ["Position"] as "Path"
                                   from "Categories"
                                   where "ParentId" is null
                
                                   union all
                
                                   select c."Id",
                                          c."Name",
                                          "Level" + 1,
                                          c."Position",
                                          c."ParentId",
                                          array_append("Path", c."Position") as Path
                                   from "Categories" c
                                            inner join categories cats on cats."Id" = c."ParentId"),
                    ancestry as (select "Id", "Name", "ParentId", "Id" as "AncestorId"
                                 from "Categories"
                
                                 union all
                
                                 select c."Id", a."Name", c."ParentId", "AncestorId"
                                 from "Categories" c
                                          inner join ancestry a on a."Id" = c."ParentId"),
                    totals as (select C."Id",
                                      C."Name",
                                      C."ParentId",
                                      extract(year from "Timestamp")  as "Year",
                                      extract(month from "Timestamp") as "Month",
                                      sum(T."Amount")                 as "Total"
                               from "Categories" C
                                        left join "Transactions" T on C."Id" = T."CategoryId"
                               group by C."Id", C."Name", extract(year from "Timestamp"), extract(month from "Timestamp")),
                    categoryTotals as (select a."AncestorId" as "CategoryId", t."Year", t."Month", sum(t."Total") as "Total"
                                       from ancestry as a
                                                inner join totals t on t."Id" = a."Id"
                                       group by a."AncestorId", t."Year", t."Month")
                select ct."CategoryId" as "Id", c."Name", c."Level", ct."Total" as "TreeTotal", coalesce(t."Total", 0) as "CategoryTotal", ct."Year", ct."Month"
                from categoryTotals ct
                         inner join categories c on c."Id" = ct."CategoryId"
                         left join totals t on t."Id" = ct."CategoryId" and t."Year" = ct."Year" and t."Month" = ct."Month"
                where ct."Year" is not null
                  and ct."Month" is not null
                order by c."Path", "Year", "Month"
             """)
            .HasNoKey();
    }
}