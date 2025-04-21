using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.GetCashFlow;

[Handler]
[MapGet("/cashflow")]
public static partial class Get
{
    public sealed record Query
    {
        public required int Year { get; init; }
    }

    public sealed record Response
    {
        public sealed record Entry
        {
            public required int Id { get; init; }

            public required string Name { get; init; }

            public required IEnumerable<decimal> TreeTotals { get; init; }

            public required IEnumerable<decimal> CategoryTotals { get; init; }

            public required decimal YearTreeTotal { get; init; }

            public required decimal YearCategoryTotal { get; init; }

            public required IEnumerable<Entry> Subcategories { get; init; }
        }

        public sealed record TotalsEntry
        {
            public required IEnumerable<decimal> Totals { get; init; }

            public required decimal YearTotal { get; init; }
        }

        public required IEnumerable<Entry> Categories { get; init; }

        public required TotalsEntry Totals { get; init; }
    }

    private static Response.Entry Map(TreeEntry entry)
    {
        return new Response.Entry
        {
            Id = entry.Id,
            Name = entry.Name,
            TreeTotals = Enumerable.Range(1, 12).Select(i => entry.TreeTotals.GetValueOrDefault(i, 0)),
            CategoryTotals = Enumerable.Range(1, 12).Select(i => entry.CategoryTotals.GetValueOrDefault(i, 0)),
            YearTreeTotal = entry.TreeTotals.Values.Sum(),
            YearCategoryTotal = entry.CategoryTotals.Values.Sum(),
            Subcategories = entry.Children.Select(Map)
        };
    }

    private record TreeEntry(
        int Id,
        string Name,
        Dictionary<int, decimal> TreeTotals,
        Dictionary<int, decimal> CategoryTotals,
        List<TreeEntry> Children);

    private static (TreeEntry, int) GetTreeEntry(int idx, List<CashFlowTreeEntry> entries)
    {
        CashFlowTreeEntry entry;
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

    private static async ValueTask<Response> HandleAsync(Query query, SstDbContext ctx,
        CancellationToken token)
    {
        var entries = await ctx.Set<CashFlowTreeEntry>()
            .Where(t => t.Year == query.Year)
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

        var response = new Response
        {
            Categories = output.Select(Map),
            Totals = new Response.TotalsEntry
            {
                Totals = monthTotals,
                YearTotal = monthTotals.Sum()
            }
        };

        return response;
    }
}