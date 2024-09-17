using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.GetCashFlow;

public class GetCashFlowEndpoint : Endpoint<GetCashFlowRequest>
{
    public required GetCashFlowQuery.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Get("/cashflow");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCashFlowRequest req, CancellationToken ct)
    {
        var response = await Handler.HandleAsync(new GetCashFlowQuery.Command
        {
            Year = req.Year
        });
        await SendOkAsync(response);
    }
}