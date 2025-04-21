using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Immediate.Validations.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Exceptions;
using Sst.Database;

namespace Sst.Api.Features.Accounts;

[Handler]
[MapPut("/accounts/{id}")]
public static partial class Update
{
    [Validate]
    public sealed partial record Command : IValidationTarget<Command>
    {
        [Validate]
        public sealed partial record CommandBody : IValidationTarget<CommandBody>
        {
            [MaxLength(100)]
            public required string Name { get; init; }
        }

        [FromRoute]
        public required int Id { get; init; }

        [FromBody]
        public required CommandBody Body { get; init; }
    }

    private static async ValueTask<NoContent> HandleAsync(
        [AsParameters]
        Command command,
        SstDbContext ctx,
        CancellationToken token)
    {
        var affected = await ctx.Accounts
            .Where(a => a.Id == command.Id)
            .ExecuteUpdateAsync(a => a.SetProperty(aa => aa.Name, command.Body.Name), token);

        if (affected == 0)
            throw new NotFoundException();

        return TypedResults.NoContent();
    }
}