using EntityFramework.Exceptions.Common;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Exceptions;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.Categories;

[Handler]
[MapPost("/categories")]
public static partial class Create
{
    public sealed record Command
    {
        public required string Name { get; init; }
    }

    private static async ValueTask<NoContent> HandleAsync(
        Command req,
        SstDbContext ctx,
        CancellationToken token)
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
            throw new ValidationException($"Category '{req.Name}' already exists");
        }

        return TypedResults.NoContent();
    }
}