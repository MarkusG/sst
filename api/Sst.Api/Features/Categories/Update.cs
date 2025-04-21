using EntityFramework.Exceptions.Common;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Immediate.Validations.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Exceptions;
using Sst.Database;
using ValidationException = Sst.Api.Exceptions.ValidationException;

namespace Sst.Api.Features.Categories;

[Handler]
[MapPut("/categories/{id}")]
public static partial class Update
{
    [Validate]
    public sealed partial record Command : IValidationTarget<Command>
    {
        [Validate]
        public sealed partial record CommandBody : IValidationTarget<CommandBody>
        {
            public required string Name { get; init; }

            public required int Position { get; init; }

            public required int? ParentId { get; init; }
        }

        [FromRoute]
        public required int Id { get; init; }

        [FromBody]
        public required CommandBody Body { get; init; }

        private static void AdditionalValidations(ValidationResult errors, Command target)
        {
            errors.Add(() => NotEqualAttribute.ValidateProperty(target.Id, target.Body.ParentId), "Cannot make a category its own child");
        }
    }

    private static async ValueTask<NoContent> HandleAsync(
        [AsParameters]
        Command req,
        SstDbContext ctx,
        CancellationToken token)
    {
        var category = await ctx.Categories
            .FirstOrDefaultAsync(c => c.Id == req.Id, token);

        if (category is null)
            throw new NotFoundException();

        category.Name = req.Body.Name;

        // if no movement needs to happen, we're done
        if (category.ParentId == req.Body.ParentId && category.Position == req.Body.Position)
        {
            await ctx.SaveChangesAsync(token);
            return TypedResults.NoContent();
        }

        var targetIsDescendantOfSelf = await ctx.Database.SqlQuery<bool>(
            $"""
             with recursive categories as (
             select "Id", "ParentId"
             from "Categories"
             where "Id" = {req.Body.ParentId} 

             union all

             select c."Id", c."ParentId"
             from "Categories" c
             inner join categories cats on cats."ParentId" = c."Id"
             ) select exists(select 1 from categories where "Id" = {req.Id}) as "Value"
             """).FirstAsync(token);

        if (targetIsDescendantOfSelf)
            throw new ValidationException("Requested parent category is a descendant of the category to be moved");

        var siblings = await ctx.Categories
            .Where(c => c.ParentId == req.Body.ParentId)
            .OrderBy(c => c.Position)
            .ToListAsync(token);

        var maxPosition = siblings
            .Select(c => c.Position)
            .DefaultIfEmpty(0)
            .Max();

        if (req.Body.Position > maxPosition + 1)
            throw new ValidationException($"Invalid position. Maximum valid position for this category is {maxPosition + 1}");

        // case 1: moving within the same category
        if (category.ParentId == req.Body.ParentId)
        {
            // remove category
            siblings.Remove(category);

            // re-insert category at new position
            if (req.Body.Position == siblings.Select(c => c.Position).Max() + 1)
                siblings.Add(category);
            else if (req.Body.Position <= category.Position)
                siblings.Insert(req.Body.Position - 1, category);
            else
                siblings.Insert(req.Body.Position - 2, category);

            // update positions
            foreach (var (c, i) in siblings.Select((c, i) => (c, i)))
                c.Position = i + 1;
        }
        // case 2: moving between categories
        else
        {
            // insert category at new position
            siblings.Insert(req.Body.Position - 1, category);

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

        category.ParentId = req.Body.ParentId;

        try
        {
            await ctx.SaveChangesAsync(token);
        }
        catch (ReferenceConstraintException)
        {
            throw new ValidationException("Requested parent category does not exist");
        }

        return TypedResults.NoContent();
    }
}