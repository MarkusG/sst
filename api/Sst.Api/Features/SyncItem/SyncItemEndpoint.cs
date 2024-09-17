using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.SyncItem;

public class SyncItemEndpoint : Endpoint<SyncItemRequest>
{
    public required SyncItemCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Post("/items/{itemId}/sync");
        // apparently necessary for POSTs with empty bodies
        Description(x => x.Accepts<SyncItemRequest>());
        AllowAnonymous();
    }

    public override async Task HandleAsync(SyncItemRequest req, CancellationToken ct)
    {
        await Handler.HandleAsync(new SyncItemCommand.Command
        {
            ItemId = req.ItemId
        });
        await SendOkAsync();
    }
}