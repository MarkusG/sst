using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Features.Transactions.Mappers;
using Sst.Database;

namespace Sst.Api.Features.Transactions;

[Handler]
[MapPost("/import/preview")]
public static partial class Preview
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint.DisableAntiforgery();

    public sealed record Query
    {
        [FromForm]
        public required IFormFile File { get; init; }
        
        [FromForm]
        public required int AccountId { get; init; }
    }

    public sealed record Response
    {
        public required DateTimeOffset Timestamp { get; init; }

        public required string Description { get; init; }

        public required decimal Amount { get; init; }

        public required bool Skipped { get; init; }
    }

    private static async ValueTask<IEnumerable<Response>> HandleAsync(
        Query query,
        SstDbContext ctx,
        TransactionMapperProvider provider,
        CancellationToken token)
    {
        var reader = new StreamReader(query.File.OpenReadStream());
        var csv = await reader.ReadToEndAsync(token);

        if (provider.TryGetMapper(csv, out var mapper))
        {
            var newTransactions = mapper.GetTransactions(csv);
            var earliest = newTransactions.OrderBy(t => t.Timestamp).FirstOrDefault();

            if (earliest is null)
                return [];

            var existingTransactions = await ctx.Transactions
                .Where(t => t.AccountId == query.AccountId)
                .Where(t => t.Timestamp >= earliest.Timestamp)
                .ToListAsync(token);

            return newTransactions.Select(t => new Response
            {
                Timestamp = t.Timestamp!.Value,
                Description = t.Description,
                Amount = t.Amount,
                Skipped = existingTransactions.Any(tt =>
                    tt.Timestamp == t.Timestamp
                    && tt.Description == t.Description
                    && tt.Amount == t.Amount)
            })
            .OrderByDescending(t => t.Timestamp);
        }

        return [];
    }
}