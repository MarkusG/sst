using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Database;

namespace Sst.Api.Features.Categories;

[Handler]
[MapGet("/categories")]
public static partial class GetAll
{
    public sealed record Response
    {
        public required IEnumerable<string> Categories { get; init; }
    }

    private static async ValueTask<Response> HandleAsync(object _, SstDbContext ctx, CancellationToken token)
    {
        var categories = await ctx.Categories
            .Select(t => t.Name)
            .ToListAsync(token);

        return new Response
        {
            Categories = categories
        };
    }
}