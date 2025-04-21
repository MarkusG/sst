using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Exceptions;
using Sst.Api.Services;
using Sst.Database;

namespace Sst.Api.Features.Categorizations;

[Handler]
[MapPut("/categorizations/{id}")]
public static partial class Update
{
    public sealed record Command
    {
        public sealed record CommandBody
        {
            public required string Category { get; init; }
        }

        [FromRoute]
        public required int Id { get; init; }

        [FromBody]
        public required CommandBody Body { get; init; }
    }

    private static async ValueTask<NoContent> HandleAsync(
        [AsParameters]
        Command req,
        SstDbContext ctx,
        CategoryService categoryService,
        CancellationToken token)
    {
        var categorization = await ctx.Categorizations.FirstOrDefaultAsync(cz => cz.Id == req.Id, token);

        if (categorization is null)
            throw new NotFoundException();

        var categoryId = await categoryService.GetOrCreateCategoryIdByNameAsync(req.Body.Category, token);

        var oldId = categorization.CategoryId;
        categorization.CategoryId = categoryId;

        await ctx.SaveChangesAsync();

        await categoryService.DeleteCategoryIfEmptyAsync(oldId, token);

        return TypedResults.NoContent();
    }
}