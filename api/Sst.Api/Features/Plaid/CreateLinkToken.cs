using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.Extensions.Options;
using Sst.Plaid;
using Sst.Plaid.Endpoints.LinkTokenCreate;

namespace Sst.Api.Features.Plaid;

[Handler]
[MapGet("/link")]
public static partial class CreateLinkToken
{
    public sealed record Response
    {
        public required string LinkToken { get; init; }
    }

    private static async ValueTask<Response> HandleAsync(
        object _,
        PlaidClient client,
        IOptions<PlaidClientOptions> options,
        CancellationToken ct)
    {
        var response = await client.LinkTokenCreate(new LinkTokenCreateRequest
        {
            ClientId = options.Value.ClientId,
            Secret = options.Value.Secret,
            Products = ["transactions"]
        }, ct);

        return new Response
        {
            LinkToken = response.LinkToken
        };
    }
}