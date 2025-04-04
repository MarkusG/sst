using Immediate.Handlers.Shared;
using Sst.Database;

namespace Sst.Api.Features.GetTransactionAccounts;

[Handler]
public partial class GetTransactionAccountsQuery
{
    private static async ValueTask<IEnumerable<string>> HandleAsync(object _, SstDbContext ctx, CancellationToken token)
    {
        return [];
    }
}