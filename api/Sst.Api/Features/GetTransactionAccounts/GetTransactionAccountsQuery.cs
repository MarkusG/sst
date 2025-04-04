using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Database;

namespace Sst.Api.Features.GetTransactionAccounts;

[Handler]
public partial class GetTransactionAccountsQuery
{
    private static async ValueTask<IEnumerable<string>> HandleAsync(object _, SstDbContext ctx, CancellationToken token)
    {
        return await ctx.Transactions
            .Select(t => t.AccountName)
            .Distinct()
            .ToListAsync();
    }
}