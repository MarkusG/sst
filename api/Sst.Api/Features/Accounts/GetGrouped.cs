using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Database;

namespace Sst.Api.Features.Accounts;

[Handler]
[MapGet("/accounts/grouped")]
public static partial class GetGrouped
{
    public sealed record Response
    {
        public sealed record Group
        {
            public sealed record Account
            {
                public required int Id { get; init; }

                public required string Name { get; init; }

                public required decimal? AvailableBalance { get; init; }

                public required decimal? CurrentBalance { get; init; }
            }

            public required int? ItemId { get; init; }

            public required IEnumerable<Account> Accounts { get; init; }
        }

        public required IEnumerable<Group> Groups { get; init; }
    }

    private static async ValueTask<Response> HandleAsync(object _, SstDbContext ctx, CancellationToken token)
    {
        var accounts = await ctx.Accounts
            .ToListAsync();

        return new Response
        {
            Groups = accounts
                .GroupBy(a => a.ItemId)
                .Select(g => new Response.Group
                {
                    ItemId = g.Key,
                    Accounts = g.Select(a => new Response.Group.Account
                    {
                        Id = a.Id,
                        Name = a.Name,
                        AvailableBalance = a.AvailableBalance,
                        CurrentBalance = a.CurrentBalance
                    }).OrderBy(a => a.Name).AsEnumerable()
                })
        };
    }
}