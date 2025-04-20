using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Features.Transactions.Mappers;
using Sst.Database;

namespace Sst.Api.Features.Transactions;

[Handler]
[MapPost("/import")]
public static partial class Import
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint.DisableAntiforgery();

    public sealed record Command
    {
        [FromForm]
        public required int AccountId { get; init; }

        [FromForm]
        public required IFormFile File { get; init; }
    }

    private static async ValueTask<NoContent> HandleAsync(
        Command command,
        SstDbContext ctx,
        TransactionMapperProvider provider,
        CancellationToken token)
    {
        var reader = new StreamReader(command.File.OpenReadStream());
        var csv = await reader.ReadToEndAsync(token);

        if (provider.TryGetMapper(csv, out var mapper))
        {
            var newTransactions = mapper.GetTransactions(csv);
            var earliest = newTransactions.OrderBy(t => t.Timestamp).FirstOrDefault();

            if (earliest is null)
                return TypedResults.NoContent();

            var existingTransactions = await ctx.Transactions
                .Where(t => t.AccountId == command.AccountId)
                .Where(t => t.Timestamp >= earliest.Timestamp)
                .ToListAsync(token);

            // deduplicate
            foreach (var t in newTransactions.Where(t =>
                         !existingTransactions.Any(tt =>
                             tt.Timestamp == t.Timestamp
                             && tt.Description == t.Description
                             && tt.Amount == t.Amount)))
            {
                t.AccountId = command.AccountId;
                ctx.Transactions.Add(t);
            }
        }

        await ctx.SaveChangesAsync(token);

        return TypedResults.NoContent();
    }
}