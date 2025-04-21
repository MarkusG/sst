using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Exceptions;
using Sst.Api.Services;
using Sst.Database;

namespace Sst.Api.Features.Categorizations;

[Handler]
[MapDelete("/categorizations/{id}")]
public static partial class Delete
{
    public sealed record Command
    {
        public required int Id { get; init; }
    }

    private static async ValueTask<NoContent> HandleAsync(
        Command req,
        SstDbContext ctx,
        CategoryService categoryService,
        CancellationToken token)
    {
        var categorization = await ctx.Categorizations
            .Where(cz => cz.Id == req.Id)
            .FirstOrDefaultAsync(token);

        if (categorization is null)
            throw new NotFoundException();

        ctx.Categorizations.Remove(categorization);

        await ctx.SaveChangesAsync(token);

        // add amount to first categorization
        await ctx.Categorizations
            .Where(cz => cz.TransactionId == categorization.TransactionId && cz.Position == 0)
            .ExecuteUpdateAsync(cz => cz.SetProperty(ccz => ccz.Amount, ccz => ccz.Amount + categorization.Amount), token);

        await categoryService.DeleteCategoryIfEmptyAsync(categorization.CategoryId, token);

        return TypedResults.NoContent();
    }
}