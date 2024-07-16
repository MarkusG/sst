using Immediate.Handlers.Shared;
using Microsoft.Extensions.Options;
using Sst.Plaid;
using Sst.Plaid.Endpoints.LinkTokenCreate;

namespace Sst.Api.Features.CreateLinkToken;

[Handler]
public static partial class CreateLinkTokenCommand
{
    public record Command;
    
    private static async ValueTask<string> HandleAsync(
        Command _,
        PlaidClient client,
        IOptions<PlaidClientOptions> options,
        CancellationToken token)
    {
        var response = await client.LinkTokenCreate(new LinkTokenCreateRequest
        {
            ClientId = options.Value.ClientId,
            Secret = options.Value.Secret,
            Products = ["transactions"]
        });

        return response.LinkToken;
    }
}