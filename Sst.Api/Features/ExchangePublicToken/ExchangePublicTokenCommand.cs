using Immediate.Handlers.Shared;
using Microsoft.Extensions.Options;
using Sst.Database;
using Sst.Database.Entities;
using Sst.Plaid;
using Sst.Plaid.Endpoints.AccountsBalanceGet;
using Sst.Plaid.Endpoints.ItemPublicTokenExchange;

namespace Sst.Api.Features.ExchangePublicToken;

[Handler]
public partial class ExchangePublicTokenCommand
{
    public record Command
    {
        public required string PublicToken { get; set; }
    }

    private static async ValueTask HandleAsync(
        Command request,
        SstDbContext ctx,
        PlaidClient client,
        IOptions<PlaidClientOptions> options,
        CancellationToken ct)
    {
        var response = await client.ItemPublicTokenExchange(new ItemPublicTokenExchangeRequest
        {
            ClientId = options.Value.ClientId,
            Secret = options.Value.Secret,
            PublicToken = request.PublicToken
        }, ct);

        var item = new Item
        {
            AccessToken = response.AccessToken,
            NextCursor = null
        };

        ctx.Items.Add(item);

        var accountsResponse = await client.GetAccountBalances(new AccountsBalanceGetRequest
        {
            ClientId = options.Value.ClientId,
            Secret = options.Value.Secret,
            AccessToken = response.AccessToken
        }, ct);

        foreach (var a in accountsResponse.Accounts)
        {
            ctx.Accounts.Add(new Account
            {
                PlaidId = a.AccountId,
                Name = a.OfficialName ?? a.Name,
                Item = item
            });
        }

        await ctx.SaveChangesAsync(ct);
    }
}