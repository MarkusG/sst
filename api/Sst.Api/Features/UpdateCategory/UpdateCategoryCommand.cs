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
        
        public required int? ParentId { get; set; }
    }
    
    private static async ValueTask<bool> HandleAsync(Command req, SstDbContext ctx, CancellationToken token)
    {
        var validationCtx = ValidationContext<UpdateCategoryRequest>.Instance;
        
        if (req.ParentId == req.Id)
            validationCtx.ThrowError("Cannot make a category a child of itself");

        // get category, include current supercategory for later
        var category = await ctx.Categories
            .Include(c => c.ParentCategory)
            .ThenInclude(c => c!.Subcategories)
            .FirstOrDefaultAsync(c => c.Id == req.Id);

        if (category is null)
            return false;
        
        // get the category it's being moved into
        var target = await ctx.Categories
            .Include(c => c.Subcategories)
            .FirstOrDefaultAsync(c => c.Id == req.ParentId);

        if (target is null && req.ParentId is not null)
            validationCtx.ThrowError("Invalid parent category");

        category.Name = req.Name;

        if (req.ParentId == category.ParentId && req.Position == category.Position)
        {
            await ctx.SaveChangesAsync(token);
            return true;
        }
        
        // get the cateogry's new siblings
        var siblings = target?.Subcategories ??
                       await ctx.Categories.Where(c => c.ParentId == null).ToListAsync();
        var orderedSiblings = siblings.OrderBy(c => c.Position).ToList();
        
        if (orderedSiblings.Remove(category))
        {
            if (req.Position == orderedSiblings.Select(c => c.Position).DefaultIfEmpty().Max() + 1)
                orderedSiblings.Add(category);
            else if (req.Position <= category.Position)
                orderedSiblings.Insert(req.Position - 1, category);
            else
                orderedSiblings.Insert(req.Position - 2, category);
        }
        else
        {
            if (req.Position == orderedSiblings.Select(c => c.Position).DefaultIfEmpty().Max() + 1)
                orderedSiblings.Add(category);
            else
                orderedSiblings.Insert(req.Position - 1, category);
        }
        
        foreach (var (c, i) in orderedSiblings.Select((c, i) => (c, i)))
            c.Position = i + 1;
        
        category.ParentId = req.ParentId;
        
        await ctx.SaveChangesAsync(token);
        return true;
    }
}