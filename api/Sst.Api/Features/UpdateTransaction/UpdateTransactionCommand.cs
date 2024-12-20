using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Database;

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
                transaction.CategoryId = (await ctx.Database.SqlQuery<int>(
                    $"""
                     insert into "Categories" ("Name", "Position", "ParentId")
                     select {req.Category}, coalesce(max("Position") + 1, 1), null
                     from "Categories"
                     where "ParentId" is null
                     returning "Id";
                     """
                ).ToListAsync()).First();
            }
        }
        else if (transaction.CategoryId is not null)
        {
            // if category has no more transactions or children, delete it
            var category = await ctx.Categories
                .Where(c => c.Id == transaction.CategoryId)
                .Select(c => new
                {
                    Category = c,
                    TransactionCount = c.Transactions.Count,
                    ChildCount = c.Subcategories.Count
                })
                .FirstOrDefaultAsync(token);

            if (category?.TransactionCount == 1 && category.ChildCount == 0)
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
        return true;
    }
}