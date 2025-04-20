using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Immediate.Validations.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.CreateAccount;

[Handler]
[MapPost("/accounts")]
public static partial class CreateAccount
{
    [Validate]
    public sealed partial record Command : IValidationTarget<Command>
    {
        [MaxLength(100)]
        public required string Name { get; init; }
    }

    private static async ValueTask<NoContent> HandleAsync(
        Command command,
        SstDbContext ctx,
        CancellationToken token)
    {
        ctx.Accounts.Add(new Account
        {
            Name = command.Name,
            PlaidId = null,
            AvailableBalance = null,
            CurrentBalance = null
        });

        await ctx.SaveChangesAsync(token);

        return TypedResults.NoContent();
    }
}