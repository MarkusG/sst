using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.GetTransactions;

public class GetTransactionsEndpoint : Endpoint<GetTransactionsRequest>
{
    public required GetTransactionsQuery.Handler Handler { get; set; }

    public override void Configure()
    {
        Get("/transactions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTransactionsRequest req, CancellationToken ct)
    {
        var response = await Handler.HandleAsync(new GetTransactionsQuery.Query
        {
            Page = req.Page,
            PageSize = req.PageSize,
            SortDirection = req.SortDirection,
            SortField = req.SortField
        });

        await SendOkAsync(response);
    }
}