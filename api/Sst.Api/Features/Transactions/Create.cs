using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.Transactions;

[Handler]
[MapPost("/transactions")]
public static partial class Create
{
    public sealed record Command
    {
        public required DateTimeOffset Timestamp { get; init; }
        
        public required string Account { get; init; }

        public required decimal Amount { get; init; }

        public required string Description { get; init; }

        public required string? Category { get; init; }
    }

    private static async ValueTask<NoContent> HandleAsync(Command req, SstDbContext ctx, CancellationToken token)
    {
        var transaction = new Transaction
        {
            PlaidId = null,
            Currency = "USD",
            Timestamp = req.Timestamp,
            Amount = req.Amount,
            Description = req.Description,
            AccountId = (await ctx.Accounts.FirstOrDefaultAsync(a => a.Name == req.Account))?.Id ?? 0
        };

        if (req.Category is not null)
        {
            var category = await ctx.Categories.FirstOrDefaultAsync(c => c.Name == req.Category);
            if (category is null)
            {
                var rootCategories = await ctx.Categories
                    .Where(c => c.ParentId == null)
                    .ToListAsync();

                foreach (var c in rootCategories)
                    c.Position++;

                category = new Category
                {
                    Name = req.Category,
                    Position = 1,
                    ParentId = null
                };
            }

            transaction.Categorizations.Add(new Categorization
            {
                TransactionId = 0,
                CategoryId = 0,
                Amount = req.Amount,
                Category = category,
                Position = 0
            });
        }

        ctx.Transactions.Add(transaction);

        await ctx.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}