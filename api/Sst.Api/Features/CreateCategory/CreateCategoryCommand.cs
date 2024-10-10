using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Contracts.Requests;
using Sst.Contracts.Responses;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.CreateCategory;

[Handler]
public partial class CreateCategoryCommand
{
    public record Command
    {
        public required string Name { get; set; }
    }
    
    private static async ValueTask<CategoryTreeEntryResponse> HandleAsync(Command req, SstDbContext ctx, CancellationToken token)
    {
        var rootCategories = await ctx.Categories
            .Where(c => c.ParentId == null)
            .ToListAsync(token);

        foreach (var c in rootCategories)
            c.Position++;

        var category = new Category
        {
            Name = req.Name,
            Position = 1,
            ParentId = null
        };

        ctx.Categories.Add(category);
        try
        {
            await ctx.SaveChangesAsync(token);
        }
        catch (UniqueConstraintException)
        {
            var validationCtx = ValidationContext<CreateCategoryRequest>.Instance;
            validationCtx.ThrowError($"Category '{req.Name}' already exists");
        }

        return new CategoryTreeEntryResponse
        {
            Id = category.Id,
            Name = category.Name,
            ParentId = null,
            Position = 1,
            Subcategories = []
        };
    }
}