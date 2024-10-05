using System.Data;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.UpdateTransaction;

[Handler]
public partial class UpdateTransactionCommand
{
    public record Command
    {
        public int Id { get; set; }

        public required DateTimeOffset? Timestamp { get; set; }

        public required decimal Amount { get; set; }

        public required string Description { get; set; }

        public required string Account { get; set; }

        public required string? Category { get; set; }
    }

    private static async ValueTask<bool> HandleAsync(Command req, SstDbContext ctx, CancellationToken token)
    {
        // explicit transaction to prevent a race condition between getting the max position and inserting the new category
        await using var dbTransaction = await ctx.Database.BeginTransactionAsync(IsolationLevel.Serializable, token);
        var transaction = await ctx.Transactions
            .FirstOrDefaultAsync(t => t.Id == req.Id, token);

        if (transaction is null)
            return false;

        if (req.Category is not null)
        {
            // get requested category
            var category = await ctx.Categories
                .FirstOrDefaultAsync(c => c.Name == req.Category, token);

            // if category exists, put the transaction into it
            if (category is { Id: var id })
            {
                transaction.CategoryId = id;
            }
            // else, create the category and put the transaction into it
            else
            {
                var position = 1;
                try
                {
                    position = await ctx.Categories.Where(c => c.SuperCategoryId == null)
                        .Select(c => c.Position)
                        .MaxAsync(token) + 1;
                }
                catch
                {
                    // ignored
                }

                transaction.Category = new Category
                {
                    Name = req.Category,
                    Position = position,
                    SuperCategoryId = null
                };
            }
        }
        else if (transaction.CategoryId is not null)
        {
            // if category has no more transactions, delete it
            var category = await ctx.Categories
                .Where(c => c.Id == transaction.CategoryId)
                .Select(c => new
                {
                    Category = c,
                    TransactionCount = c.Transactions.Count
                })
                .FirstOrDefaultAsync(token);

            if (category?.TransactionCount == 1)
            {
                ctx.Categories.Remove(category.Category);
            }

            transaction.CategoryId = null;
        }

        transaction.Timestamp = req.Timestamp;
        transaction.Amount = req.Amount;
        transaction.Description = req.Description;
        transaction.AccountName = req.Account;

        await ctx.SaveChangesAsync(token);
        await dbTransaction.CommitAsync(token);
        return true;
    }
}