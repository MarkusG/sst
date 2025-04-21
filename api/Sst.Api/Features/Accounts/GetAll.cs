using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Database;

namespace Sst.Api.Features.Accounts;

[Handler]
[MapGet("/accounts")]
public static partial class GetAll
{
    public sealed record Response
    {
        public required int Id { get; init; }

        public required string Name { get; init; }

        public required int TransactionCount { get; init; }

        public required bool IsPlaid { get; init; }
    }

    private static async ValueTask<IEnumerable<Response>> HandleAsync(
        object _,
        SstDbContext ctx,
        CancellationToken token)
    {
        return await ctx.Accounts
            .Select(a => new Response
            {
                Id = a.Id,
                Name = a.Name,
                TransactionCount = a.Transactions.Count,
                IsPlaid = a.PlaidId != null
            })
            .ToListAsync(token);
    }
}