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
        var transaction = await ctx.Transactions
            .Include(t => t.Categorizations)
            .FirstOrDefaultAsync(t => t.Id == req.Id, token);

        if (transaction is null)
            return false;

        if (req.Category is not null)
        {
            // get requested category
            var category = await ctx.Categories.FirstOrDefaultAsync(c => c.Name == req.Category, token);

            // if category exists, put the transaction into it
            if (category is { Id: var id })
            {
                transaction.Categorizations = [new Categorization
                {
                    TransactionId = 0,
                    CategoryId = id,
                    Amount = transaction.Amount,
                    Position = 0
                }];
            }
            // else, create the category and put the transaction into it
            else
            {
                var categoryId = (await ctx.Database.SqlQuery<int>(
                    $"""
                     insert into "Categories" ("Name", "Position", "ParentId")
                     select {req.Category}, coalesce(max("Position") + 1, 1), null
                     from "Categories"
                     where "ParentId" is null
                     returning "Id";
                     """
                ).ToListAsync()).First();
                
                transaction.Categorizations = [new Categorization
                {
                    TransactionId = 0,
                    CategoryId = categoryId,
                    Amount = transaction.Amount,
                    Position = 0
                }];
            }
        }
        else if (transaction.Categorizations is [var transactionCategory])
        {
            // if category has no more transactions or children, delete it
            var category = await ctx.Categories
                .Where(c => c.Id == transactionCategory.CategoryId)
                .Select(c => new
                {
                    Category = c,
                    CategorizationCount = ctx.Categorizations.Count(cc => cc.CategoryId == c.Id),
                    ChildCount = c.Subcategories.Count
                })
                .FirstOrDefaultAsync(token);
            
            Console.WriteLine($"category: {category.Category.Name}");
            Console.WriteLine($"categorizations: {category.CategorizationCount}");
            Console.WriteLine($"children: {category.ChildCount}");

            if (category?.CategorizationCount == 1 && category.ChildCount == 0)
            {
                ctx.Categories.Remove(category.Category);
            }

            transaction.Categorizations = [];
        }

        transaction.Timestamp = req.Timestamp;
        transaction.Amount = req.Amount;
        transaction.Description = req.Description;

        await ctx.SaveChangesAsync(token);
        return true;
    }
}