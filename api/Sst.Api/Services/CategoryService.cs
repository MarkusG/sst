using Microsoft.EntityFrameworkCore;
using Sst.Database;

namespace Sst.Api.Services;

public class CategoryService(SstDbContext ctx)
{
    public async Task<int> GetOrCreateCategoryIdByName(string name, CancellationToken token = default)
    {
        // get requested category
        var category = await ctx.Categories.FirstOrDefaultAsync(c => c.Name == name, token);

        if (category is { Id: var id })
            return id;

        return (await ctx.Database.SqlQuery<int>(
            $"""
             insert into "Categories" ("Name", "Position", "ParentId")
             select {name}, coalesce(max("Position") + 1, 1), null
             from "Categories"
             where "ParentId" is null
             returning "Id";
             """
        ).ToListAsync()).First();
    }
}