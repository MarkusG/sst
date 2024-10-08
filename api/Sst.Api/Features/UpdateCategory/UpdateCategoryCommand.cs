using EntityFramework.Exceptions.Common;
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

        var category = await ctx.Categories
            .FirstOrDefaultAsync(c => c.Id == req.Id, token);

        if (category is null)
            return false;

        category.Name = req.Name;

        // if no movement needs to happen, we're done
        if (category.ParentId == req.ParentId && category.Position == req.Position)
        {
            await ctx.SaveChangesAsync(token);
            return true;
        }

        var targetIsDescendantOfSelf = await ctx.Database.SqlQuery<bool>(
            $"""
             with recursive categories as (
             select "Id", "ParentId"
             from "Categories"
             where "Id" = {req.ParentId} 

             union all

             select c."Id", c."ParentId"
             from "Categories" c
             inner join categories cats on cats."ParentId" = c."Id"
             ) select exists(select 1 from categories where "Id" = {req.Id}) as "Value"
             """).FirstAsync(token);
        
        if (targetIsDescendantOfSelf)
            validationCtx.ThrowError("Cannot make a category a descendant of itself");

        var siblings = await ctx.Categories
            .Where(c => c.ParentId == req.ParentId)
            .OrderBy(c => c.Position)
            .ToListAsync(token);

        try
        {
            // case 1: moving within the same category
            if (category.ParentId == req.ParentId)
            {
                // remove category
                siblings.Remove(category);

                // re-insert category at new position
                if (req.Position == siblings.Select(c => c.Position).Max() + 1)
                    siblings.Add(category);
                else if (req.Position <= category.Position)
                    siblings.Insert(req.Position - 1, category);
                else
                    siblings.Insert(req.Position - 2, category);

                // update positions
                foreach (var (c, i) in siblings.Select((c, i) => (c, i)))
                    c.Position = i + 1;
            }
            // case 2: moving between categories
            else
            {
                // insert category at new position
                siblings.Insert(req.Position - 1, category);

                // update positions
                foreach (var (c, i) in siblings.Select((c, i) => (c, i)))
                    c.Position = i + 1;

                // get old siblings
                var oldSiblings = await ctx.Categories
                    .Where(c => c.ParentId == category.ParentId && c.Id != req.Id)
                    .OrderBy(c => c.Position)
                    .ToListAsync(token);

                // update old sibling positions
                foreach (var (c, i) in oldSiblings.Select((c, i) => (c, i)))
                    c.Position = i + 1;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            validationCtx.ThrowError("Invalid position");
        }

        category.ParentId = req.ParentId;

        try
        {
            await ctx.SaveChangesAsync(token);
        }
        catch (ReferenceConstraintException)
        {
            validationCtx.ThrowError("Invalid parent category");
        }

        return true;
    }
}