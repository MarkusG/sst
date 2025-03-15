using Microsoft.EntityFrameworkCore;
using Sst.Database;

namespace Sst.Api.Services;

public class CategoryService(SstDbContext ctx)
{
    public async Task<int> GetOrCreateCategoryIdByNameAsync(string name, CancellationToken token = default)
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

    public async Task DeleteCategoryIfEmptyAsync(int id, CancellationToken token = default)
    {
        // FK constraint prevents us from deleting a category with children
        await ctx.Categories
            .Where(c => c.Id == id)
            .Where(c => !ctx.Categorizations.Any(cz => cz.CategoryId == c.Id))
            .ExecuteDeleteAsync(token);
    }
}