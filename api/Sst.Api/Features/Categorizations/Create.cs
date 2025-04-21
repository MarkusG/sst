using FastEndpoints;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Exceptions;
using Sst.Api.Services;
using Sst.Database;
using Sst.Database.Entities;

// ReSharper disable VariableHidesOuterVariable

namespace Sst.Api.Features.Categorizations;

[Handler]
[MapPut("/transactions/{transactionId}/categorizations/{category}")]
public static partial class Create
{
    public sealed record Command
    {
        public sealed record CommandBody
        {
            public required decimal Amount { get; init; }

            public required int Position { get; init; }
        }

        public required int TransactionId { get; init; }

        public required string Category { get; init; }

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
        var categoryId = await categoryService.GetOrCreateCategoryIdByNameAsync(req.Category, token);

        var transaction = await ctx.Transactions
            .Include(t => t.Categorizations.OrderBy(cz => cz.Position))
            .FirstOrDefaultAsync(t => t.Id == req.TransactionId, token);

        if (transaction is null)
            throw new NotFoundException();

        var categorization = transaction.Categorizations
            .FirstOrDefault(cz => cz.CategoryId == categoryId);

        if (categorization is not null)
        {
            categorization.Amount = req.Body.Amount;
            await ctx.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        if (transaction.Categorizations is [var cz, ..])
            cz.Amount -= req.Body.Amount;

        ctx.Categorizations.Add(new Categorization
        {
            TransactionId = req.TransactionId,
            Amount = req.Body.Amount,
            CategoryId = categoryId,
            Position = req.Body.Position
        });

        await ctx.Categorizations
            .Where(cz => cz.TransactionId == req.TransactionId && cz.Position >= req.Body.Position)
            .ExecuteUpdateAsync(cz => cz.SetProperty(ccz => ccz.Position, ccz => ccz.Position + 1), token);

        await ctx.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}