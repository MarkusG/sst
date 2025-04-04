using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Services;
using Sst.Database;

namespace Sst.Api.Features.DeleteCategorization;

[Handler]
public partial class DeleteCategorizationCommand
{
    public record Command
    {
        public required int Id { get; set; }
    }
    
    private static async ValueTask<bool> HandleAsync(Command req, SstDbContext ctx, CategoryService categoryService, CancellationToken token)
    {
        var categorization = await ctx.Categorizations
            .Where(cz => cz.Id == req.Id)
            .FirstOrDefaultAsync(token);

        if (categorization is null)
            return false;

        ctx.Categorizations.Remove(categorization);
        
        await ctx.SaveChangesAsync(token);

        // add amount to first categorization
        await ctx.Categorizations
            .Where(cz => cz.TransactionId == categorization.TransactionId && cz.Position == 0)
            .ExecuteUpdateAsync(cz => cz.SetProperty(ccz => ccz.Amount, ccz => ccz.Amount + categorization.Amount), token);
        
        await categoryService.DeleteCategoryIfEmptyAsync(categorization.CategoryId, token);
        
        return true;
    }
}