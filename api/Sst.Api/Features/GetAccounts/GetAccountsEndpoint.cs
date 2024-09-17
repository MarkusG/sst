using FastEndpoints;

namespace Sst.Api.Features.GetAccounts;

public class GetAccountsEndpoint : EndpointWithoutRequest
{
    public required GetAccountsQuery.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Get("/accounts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await Handler.HandleAsync(null);
        await SendOkAsync(response);
    }
}