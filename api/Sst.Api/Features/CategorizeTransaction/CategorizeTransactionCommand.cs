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
        var categoryId = await categoryService.GetOrCreateCategoryIdByNameAsync(req.Category, token);

        var transaction = await ctx.Transactions
            .Include(t => t.Categorizations.OrderBy(cz => cz.Position))
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

        if (transaction.Categorizations is [var cz, ..])
            cz.Amount -= req.Amount;

        ctx.Categorizations.Add(new Categorization
        {
            TransactionId = req.TransactionId,
            Amount = req.Amount,
            CategoryId = categoryId,
            Position = req.Position
        });

        await ctx.Categorizations
            .Where(cz => cz.TransactionId == req.TransactionId && cz.Position >= req.Position)
            .ExecuteUpdateAsync(cz => cz.SetProperty(ccz => ccz.Position, ccz => ccz.Position + 1), token);

        await ctx.SaveChangesAsync();
    }

    public record Command
    {
        public required int TransactionId { get; set; }

        public required decimal Amount { get; set; }

        public required string Category { get; set; }
        
        public required int Position { get; set; }
    }
}