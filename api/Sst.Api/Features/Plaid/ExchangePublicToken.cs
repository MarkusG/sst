using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Sst.Database;
using Sst.Database.Entities;
using Sst.Plaid;
using Sst.Plaid.Endpoints.AccountsBalanceGet;
using Sst.Plaid.Endpoints.ItemPublicTokenExchange;

namespace Sst.Api.Features.ExchangePublicToken;

[Handler]
[MapPost("/exchange")]
public partial class ExchangePublicToken
{
    public record Command
    {
        public required string PublicToken { get; init; }
    }

    private static async ValueTask<NoContent> HandleAsync(
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
                AvailableBalance = a.Balances.Available,
                CurrentBalance = a.Balances.Current,
                Item = item
            });
        }

        await ctx.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }
}