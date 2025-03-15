using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.DeleteCategorization;

public class DeleteCategorizationEndpoint : Endpoint<DeleteCategorizationRequest>
{
    public required DeleteCategorizationCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Delete("/categorizations/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteCategorizationRequest req, CancellationToken ct)
    {
        var found = await Handler.HandleAsync(new DeleteCategorizationCommand.Command
        {
            Id = req.Id
        });

        if (!found)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}