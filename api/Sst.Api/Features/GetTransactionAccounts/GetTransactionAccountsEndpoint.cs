using FastEndpoints;

namespace Sst.Api.Features.GetTransactionAccounts;

public class GetTransactionAccountsEndpoint : EndpointWithoutRequest
{
    public required GetTransactionAccountsQuery.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Get("transaction-accounts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var accounts = await Handler.HandleAsync(null, ct);

        await SendOkAsync(accounts);
    }
}