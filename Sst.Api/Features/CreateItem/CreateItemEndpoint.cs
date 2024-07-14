using FastEndpoints;

namespace Sst.Api.Features.CreateItem;

public class CreateItemEndpoint : Endpoint<CreateItemRequest, CreateItemResponse>
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
        });

        await SendAsync(new CreateItemResponse
        {
            Id = itemId
        }, 201);
    }
}