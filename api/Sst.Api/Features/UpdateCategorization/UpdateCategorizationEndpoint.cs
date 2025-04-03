using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.UpdateCategorization;

public class UpdateCategorizationEndpoint : Endpoint<UpdateCategorizationRequest>
{
    public required UpdateCategorizationCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Put("/categorizations/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCategorizationRequest req, CancellationToken ct)
    {
        var found = await Handler.HandleAsync(new UpdateCategorizationCommand.Command
        {
            Id = req.Id,
            Category = req.Category
        });

        if (!found)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendNoContentAsync();
    }
}