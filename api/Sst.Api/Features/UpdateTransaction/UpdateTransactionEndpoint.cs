using FastEndpoints;

namespace Sst.Api.Features.UpdateTransaction;

public class UpdateTransactionEndpoint : Endpoint<UpdateTransactionRequest>
{
    public required UpdateTransactionCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Put("/transactions/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateTransactionRequest req, CancellationToken ct)
    {
        var success = await Handler.HandleAsync(new UpdateTransactionCommand.Command
        {
            Id = req.Id,
            Timestamp = req.Timestamp,
            Amount = req.Amount,
            Description = req.Description,
            Account = req.Account,
            Category = req.Category
        });

        if (!success)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendNoContentAsync();
    }
}