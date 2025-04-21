using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.Categories;

[Handler]
[MapGet("/categories/tree")]
public partial class GetTree
{
    public sealed record Response
    {
        public sealed record Entry
        {
            public required int Id { get; init; }

            public required string Name { get; init; }

            public required int Position { get; init; }

            public required int? ParentId { get; init; }

            public required IEnumerable<Entry> Subcategories { get; init; }
        }

        public required IEnumerable<Entry> Categories { get; init; }
    }

    private static Response.Entry Map(TreeEntry entry)
    {
        return new Response.Entry
        {
            Id = entry.Category.Id,
            Name = entry.Category.Name,
            Position = entry.Category.Position,
            ParentId = entry.Category.ParentId,
            Subcategories = entry.Children.Select(Map)
        };
    }

    private record TreeEntry(CategoryTreeEntry Category, List<TreeEntry> Children);

    private static (TreeEntry, int) GetEntryTree(int idx, List<CategoryTreeEntry> entries)
    {
        var entry = entries[idx];
        var children = new List<TreeEntry>();

        // end of entries; no children
        if (entries.Count == idx + 1)
            return (new TreeEntry(entry, []), -1);

        // get next entry
        var next = entries[++idx];

        // add next entry's children
        while (idx != entries.Count && next.Level == entry.Level + 1)
        {
            var result = GetEntryTree(idx, entries);
            children.Add(result.Item1);
            if (result.Item2 == -1)
                break;
            idx = result.Item2;
            next = entries[idx];
        }

        return (new TreeEntry(entry, children), idx);
    }

    private static async ValueTask<Response> HandleAsync(object _, SstDbContext ctx, CancellationToken token)
    {
        var entries = await ctx.Set<CategoryTreeEntry>().ToListAsync(token);
        var output = entries.Select((e, i) => new { Entry = e, Index = i })
            .Where(e => e.Entry.Level == 0)
            .Select(e => GetEntryTree(e.Index, entries).Item1)
            .ToList();

        return new Response
        {
            Categories = output.Select(Map)
        };
    }
}