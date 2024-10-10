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

    private static CashFlowTreeEntryResponse Map(TreeEntry entry)
    {
        return new CashFlowTreeEntryResponse
        {
            Id = entry.Id,
            Category = entry.Name,
            TreeTotals = Enumerable.Range(1, 12).Select(i => entry.TreeTotals.GetValueOrDefault(i, 0)),
            CategoryTotals = Enumerable.Range(1, 12).Select(i => entry.CategoryTotals.GetValueOrDefault(i, 0)),
            YearTreeTotal = entry.TreeTotals.Values.Sum(),
            YearCategoryTotal = entry.CategoryTotals.Values.Sum(),
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
            .Select(e => GetTreeEntry(e.Index, entries).Item1)
            .ToList();

        var monthTotals = Enumerable.Range(1, 12)
            .Select(i =>
                output
                    .Select(c => c.TreeTotals.GetValueOrDefault(i, 0))
                    .Sum())
            .ToList();

        var response = new CashFlowTreeResponse
        {
            Categories = output.Select(Map),
            Totals = new CashFlowTreeTotalsResponse
            {
                Totals = monthTotals,
                YearTotal = monthTotals.Sum()
            }
        };

        return response;
    }
}