using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Contracts.Responses;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.GetCashFlow;

[Handler]
public partial class GetCashFlowQuery
{
    public record Command
    {
        public required int Year { get; init; }
    }

    private static CashFlowTreeTotalResponse Map(TreeEntry entry, int month)
    {
        return new CashFlowTreeTotalResponse
        {
            Tree = entry.TreeTotals.GetValueOrDefault(month, 0),
            Category = entry.CategoryTotals.GetValueOrDefault(month, 0)
        };
    }

    private static CashFlowTreeEntryResponse Map(TreeEntry entry)
    {
        return new CashFlowTreeEntryResponse
        {
            Id = entry.Id,
            Category = entry.Name,
            January = Map(entry, 1),
            February = Map(entry, 2),
            March = Map(entry, 3),
            April = Map(entry, 4),
            May = Map(entry, 5),
            June = Map(entry, 6),
            July = Map(entry, 7),
            August = Map(entry, 8),
            September = Map(entry, 9),
            October = Map(entry, 10),
            November = Map(entry, 11),
            December = Map(entry, 12),
            TreeTotal = entry.TreeTotals.Values.Sum(),
            CategoryTotal = entry.CategoryTotals.Values.Sum(),
            Children = entry.Children.Select(c => Map(c))
        };
    }

    private record TreeEntry(
        int Id,
        string Name,
        Dictionary<int, decimal> TreeTotals,
        Dictionary<int, decimal> CategoryTotals,
        List<TreeEntry> Children);

    private static (TreeEntry, int) GetTreeEntry(int idx, List<CategoryMonthTotalTreeEntry> entries)
    {
        CategoryMonthTotalTreeEntry entry;
        var treeTotals = new Dictionary<int, decimal>();
        var categoryTotals = new Dictionary<int, decimal>();

        // insert totals
        do
        {
            entry = entries[idx++];
            treeTotals.Add(entry.Month, entry.TreeTotal);
            categoryTotals.Add(entry.Month, entry.CategoryTotal);
        } while (idx != entries.Count && entries[idx].Id == entry.Id);

        // end of entries; no children
        if (entries.Count == idx)
            return (new TreeEntry(entry.Id, entry.Name, treeTotals, categoryTotals, []), -1);

        var children = new List<TreeEntry>();

        // get next entry
        var next = entries[idx];

        // add next entry's children
        while (idx != entries.Count && next.Level == entry.Level + 1)
        {
            var result = GetTreeEntry(idx, entries);
            children.Add(result.Item1);
            if (result.Item2 == -1)
                break;
            idx = result.Item2;
            next = entries[idx];
        }

        return (new TreeEntry(entry.Id, entry.Name, treeTotals, categoryTotals, children), idx);
    }

    private static async ValueTask<CashFlowTreeResponse> HandleAsync(Command req, SstDbContext ctx,
        CancellationToken token)
    {
        var entries = await ctx.Set<CategoryMonthTotalTreeEntry>()
            .Where(t => t.Year == req.Year)
            .ToListAsync(token);

        var output = entries.Select((e, i) => new { Entry = e, Index = i })
            .Where(e => e.Entry.Level == 0)
            .GroupBy(e => e.Entry.Id)
            .Select(g => g.First())
            .Select(e => GetTreeEntry(e.Index, entries).Item1);

        var categoriesResponse = output.Select(c => Map(c)).ToList();

        var response = new CashFlowTreeResponse
        {
            Categories = categoriesResponse,
            Totals = new CashFlowTreeTotalsResponse
            {
                January = categoriesResponse.Sum(r => r.January.Tree),
                February = categoriesResponse.Sum(r => r.February.Tree),
                March = categoriesResponse.Sum(r => r.March.Tree),
                April = categoriesResponse.Sum(r => r.April.Tree),
                May = categoriesResponse.Sum(r => r.May.Tree),
                June = categoriesResponse.Sum(r => r.June.Tree),
                July = categoriesResponse.Sum(r => r.July.Tree),
                August = categoriesResponse.Sum(r => r.August.Tree),
                September = categoriesResponse.Sum(r => r.September.Tree),
                October = categoriesResponse.Sum(r => r.October.Tree),
                November = categoriesResponse.Sum(r => r.November.Tree),
                December = categoriesResponse.Sum(r => r.December.Tree),
                Total = categoriesResponse.Sum(r =>
                    r.January.Tree + r.February.Tree + r.March.Tree + r.April.Tree +
                    r.May.Tree + r.June.Tree + r.July.Tree + r.August.Tree + r.September.Tree +
                    r.October.Tree + r.November.Tree + r.December.Tree)
            }
        };

        return response;
    }
}