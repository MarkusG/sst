using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Services;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.CategorizeTransaction;

[Handler]
public partial class CategorizeTransactionCommand
{
    private static async ValueTask HandleAsync(Command req, SstDbContext ctx, CategoryService categoryService, CancellationToken token)
    {
        var categoryId = await categoryService.GetOrCreateCategoryIdByName(req.Category, token);

        var transaction = await ctx.Transactions
            .Include(t => t.Categorizations)
            .FirstOrDefaultAsync(t => t.Id == req.TransactionId, token);

        if (transaction is null)
            throw new Exception();

        var categorization = transaction.Categorizations
            .FirstOrDefault(cz => cz.CategoryId == categoryId);

        if (categorization is not null)
        {
            categorization.Amount = req.Amount;
            await ctx.SaveChangesAsync();
            return;
        }

        if (transaction.Categorizations is [var cz])
            cz.Amount = transaction.Amount - req.Amount;

        ctx.Categorizations.Add(new Categorization
        {
            TransactionId = req.TransactionId,
            Amount = req.Amount,
            CategoryId = categoryId
        });

        await ctx.SaveChangesAsync();
    }

    public record Command
    {
        public required int TransactionId { get; set; }

        public required decimal Amount { get; set; }

        public required string Category { get; set; }
    }
}