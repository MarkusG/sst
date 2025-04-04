using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.ImportTransactions;

public class ImportTransactionsEndpoint : Endpoint<ImportTransactionsRequest>
{
    public required ImportTransactionsCommand.Handler Handler { get; set; }

    public override void Configure()
    {
        Post("/import");
        AllowAnonymous();
        AllowFileUploads();
    }

    public override async Task HandleAsync(ImportTransactionsRequest req, CancellationToken ct)
    {
        await Handler.HandleAsync(new ImportTransactionsCommand.Command
        {
            AccountName = req.AccountName,
            File = req.File
        });
    }
}