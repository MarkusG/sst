using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.CreateTransaction;

public class CreateTransactionEndpoint : Endpoint<CreateTransactionRequest>
{
    public required CreateTransactionCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Post("/transactions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateTransactionRequest req, CancellationToken ct)
    {
        var response = await Handler.HandleAsync(new CreateTransactionCommand.Command
        {
            Timestamp = req.Timestamp,
            Amount = req.Amount,
            Description = req.Description,
            Account = req.Account,
            Category = req.Category
        });

        await SendOkAsync(response);
    }
}