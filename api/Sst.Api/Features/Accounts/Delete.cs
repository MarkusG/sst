using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Exceptions;
using Sst.Database;

namespace Sst.Api.Features.Accounts;

[Handler]
[MapDelete("/accounts/{id}")]
public static partial class Delete
{
    public sealed record Command
    {
        [FromRoute]
        public required int Id { get; init; }
    }

    private static async ValueTask<NoContent> HandleAsync(
        Command command,
        SstDbContext ctx,
        CancellationToken token)
    {
        var affected = await ctx.Accounts
            .Where(a => a.Id == command.Id)
            .ExecuteDeleteAsync(token);

        if (affected == 0)
            throw new NotFoundException();

        return TypedResults.NoContent();
    }
}