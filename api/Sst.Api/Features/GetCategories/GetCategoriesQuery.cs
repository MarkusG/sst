using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Contracts.Responses;
using Sst.Database;

namespace Sst.Api.Features.GetCategories;

[Handler]
public partial class GetCategoriesQuery
{
    private static async ValueTask<CategoriesResponse> HandleAsync(object _, SstDbContext ctx, CancellationToken token)
    {
        var categories = await ctx.Transactions
            .Where(t => t.Category != null)
            .Select(t => t.Category!)
            .Distinct()
            .ToListAsync(token);

        return new CategoriesResponse
        {
            Categories = categories
        };
    }
}