using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Services;
using Sst.Database;

namespace Sst.Api.Features.UpdateCategorization;

[Handler]
public partial class UpdateCategorizationCommand
{
    public record Command
    {
        public required int Id { get; set; }

        public required string Category { get; set; }
    }

    private static async ValueTask<bool> HandleAsync(Command req, SstDbContext ctx, CategoryService categoryService, CancellationToken token)
    {
        var categorization = await ctx.Categorizations.FirstOrDefaultAsync(cz => cz.Id == req.Id, token);

        if (categorization is null)
            return false;

        var categoryId = await categoryService.GetOrCreateCategoryIdByNameAsync(req.Category, token);

        var oldId = categorization.CategoryId;
        categorization.CategoryId = categoryId;

        await ctx.SaveChangesAsync();
        
        await categoryService.DeleteCategoryIfEmptyAsync(oldId, token);
        
        return true;
    }
}