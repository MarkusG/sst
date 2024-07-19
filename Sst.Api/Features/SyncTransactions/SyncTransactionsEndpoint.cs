using FastEndpoints;

namespace Sst.Api.Features.SyncTransactions;

public class SyncTransactionsEndpoint : Endpoint<SyncTransactionsRequest>
{
    public required SyncTransactionsCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Post("/sync");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SyncTransactionsRequest req, CancellationToken ct)
    {
        await Handler.HandleAsync(new SyncTransactionsCommand.Command
        {
            ItemId = req.ItemId
        }, ct);
        await SendOkAsync();
    }
}