using FastEndpoints;
using Sst.Contracts;
using Sst.Contracts.Requests;
using Sst.Contracts.Responses;

namespace Sst.Api.Features.CreateItem;

public class CreateItemEndpoint : Endpoint<CreateItemRequest, ItemResponse>
{
    public required CreateItemCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Post("/items");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateItemRequest req, CancellationToken ct)
    {
        var itemId = await Handler.HandleAsync(new CreateItemCommand.Command
        {
            AccessToken = req.AccessToken
        }, ct);

        await SendAsync(new ItemResponse
        {
            Id = itemId
        }, 201);
    }
}