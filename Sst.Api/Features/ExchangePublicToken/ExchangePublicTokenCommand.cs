using Immediate.Handlers.Shared;
using Microsoft.Extensions.Options;
using Sst.Database;
using Sst.Database.Entities;
using Sst.Plaid;
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
        CancellationToken token)
    {
        var response = await client.ItemPublicTokenExchange(new ItemPublicTokenExchangeRequest
        {
            ClientId = options.Value.ClientId,
            Secret = options.Value.Secret,
            PublicToken = request.PublicToken
        });

        ctx.Items.Add(new Item
        {
            AccessToken = response.AccessToken
        });

        await ctx.SaveChangesAsync();
    }
}