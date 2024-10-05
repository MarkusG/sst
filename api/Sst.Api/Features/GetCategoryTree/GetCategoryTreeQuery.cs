using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Contracts.Responses;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.GetCategoryTree;

[Handler]
public partial class GetCategoryTreeQuery
{
    private static CategoryTreeEntryResponse Map(TreeEntry entry)
    {
        return new CategoryTreeEntryResponse
        {
            Id = entry.Category.Id,
            Name = entry.Category.Name,
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

    private static async ValueTask<CategoryTreeResponse> HandleAsync(object _, SstDbContext ctx, CancellationToken token)
    {
        var entries = await ctx.Set<CategoryTreeEntry>().ToListAsync(token);
        var output = entries.Select((e, i) => new { Entry = e, Index = i })
            .Where(e => e.Entry.Level == 0)
            .Select(e => GetEntryTree(e.Index, entries).Item1)
            .ToList();

        return new CategoryTreeResponse
        {
            Categories = output.Select(Map)
        };
    }
}