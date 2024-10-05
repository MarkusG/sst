using System.Data;
using FastEndpoints;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Contracts.Requests;
using Sst.Database;

namespace Sst.Api.Features.UpdateCategory;

[Handler]
public partial class UpdateCategoryCommand
{
    public record Command
    {
        public required int Id { get; set; }
        
        public required string Name { get; set; }
        
        public required int Position { get; set; }
        
        public required int? SuperCategoryId { get; set; }
    }
    
    private static async ValueTask<bool> HandleAsync(Command req, SstDbContext ctx, CancellationToken token)
    {
        await using var transaction = await ctx.Database.BeginTransactionAsync(IsolationLevel.Serializable, token);

        var category = await ctx.Categories
            .Include(c => c.SuperCategory)
            .FirstOrDefaultAsync(c => c.Id == req.Id, token);

        if (category is null)
            return false;

        if (req.SuperCategoryId is not null)
        {
            var superCategory = await ctx.Categories
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync(c => c.Id == req.SuperCategoryId, token);
            
            if (superCategory is null)
            {
                var validationCtx = ValidationContext<UpdateCategoryRequest>.Instance;
                validationCtx.ThrowError("Invalid supercategory");
            }

            if (superCategory.Id != category.SuperCategoryId)
            {
                foreach (var c in superCategory.Subcategories.Where(c => c.Position >= req.Position))
                    c.Position++;
            }
            else
            {
                superCategory.Subcategories.Remove(category);
                if (req.Position == superCategory.Subcategories.Max(c => c.Position) + 1)
                    superCategory.Subcategories.Add(category);
                else
                    superCategory.Subcategories.Insert(req.Position - 1, category);
                foreach (var (c, i) in superCategory.Subcategories.Select((c, i) => (c, i)))
                    c.Position = i + 1;
            }
        }
        else
        {
            await ctx.Categories
                .Where(c => c.SuperCategoryId == null && c.Position >= req.Position)
                .ExecuteUpdateAsync(c => c.SetProperty(cc => cc.Position, cc => cc.Position + 1), token);
        }

        category.Name = req.Name;
        category.Position = req.Position;
        category.SuperCategoryId = req.SuperCategoryId;

        await ctx.SaveChangesAsync(token);
        await transaction.CommitAsync();
        return true;
    }
}