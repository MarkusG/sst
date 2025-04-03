using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Contracts.Responses;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.CreateTransaction;

[Handler]
public partial class CreateTransactionCommand
{
    private static async ValueTask<TransactionResponse> HandleAsync(Command req, SstDbContext ctx,
        CancellationToken token)
    {
        var transaction = new Transaction
        {
            PlaidId = null,
            // TODO support currency
            Currency = "USD",
            Timestamp = req.Timestamp,
            Amount = req.Amount,
            Description = req.Description,
            AccountName = req.Account
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

        return new TransactionResponse
        {
            Id = transaction.Id,
            Timestamp = transaction.Timestamp,
            Amount = transaction.Amount,
            Description = transaction.Description,
            Account = transaction.AccountName,
            Categorizations = transaction.Categorizations.Select(cz => new CategorizationResponse
            {
                Id = cz.Id,
                Amount = cz.Amount,
                Category = cz.Category!.Name,
                Position = cz.Position
            })
        };
    }

    public record Command
    {
        public required DateTimeOffset Timestamp { get; set; }

        public required decimal Amount { get; set; }

        public required string Description { get; set; }

        public required string Account { get; set; }

        public required string? Category { get; set; }
    }
}