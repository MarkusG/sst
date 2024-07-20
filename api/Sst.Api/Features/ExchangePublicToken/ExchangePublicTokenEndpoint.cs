using FastEndpoints;

namespace Sst.Api.Features.ExchangePublicToken;

public class ExchangePublicTokenEndpoint : Endpoint<ExchangePublicTokenRequest>
{
    public required ExchangePublicTokenCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Post("/exchange");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ExchangePublicTokenRequest req, CancellationToken ct)
    {
        await Handler.HandleAsync(new ExchangePublicTokenCommand.Command
        {
            PublicToken = req.PublicToken
        }, ct);

        await SendOkAsync();
    }
}