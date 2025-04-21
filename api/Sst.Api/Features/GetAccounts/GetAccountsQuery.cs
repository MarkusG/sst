using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Contracts.Responses;
using Sst.Database;

namespace Sst.Api.Features.GetAccounts;

[Handler]
public partial class GetAccountsQuery
{
    private static async ValueTask<AccountsResponse> HandleAsync(object _, SstDbContext ctx, CancellationToken token)
    {
        var accounts = await ctx.Accounts
            .ToListAsync();

        return new AccountsResponse
        {
            Groups = accounts
                .GroupBy(a => a.ItemId)
                .Select(g => new AccountGroupResponse
                {
                    ItemId = g.Key,
                    Accounts = g.Select(a => new AccountResponse
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