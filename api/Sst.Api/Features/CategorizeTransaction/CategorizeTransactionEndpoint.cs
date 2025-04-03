using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.CategorizeTransaction;

public class CategorizeTransactionEndpoint : Endpoint<CategorizeTransactionRequest>
{
    public required CategorizeTransactionCommand.Handler Handler { get; set; }

    public override void Configure()
    {
        Put("/transactions/{transactionId}/categorizations/{category}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CategorizeTransactionRequest req, CancellationToken ct)
    {
        await Handler.HandleAsync(new CategorizeTransactionCommand.Command
        {
            TransactionId = req.TransactionId,
            Amount = req.Amount,
            Category = req.Category,
            Position = req.Position
        });
    }
}