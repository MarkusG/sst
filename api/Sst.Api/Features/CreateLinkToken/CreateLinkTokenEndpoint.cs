using FastEndpoints;

namespace Sst.Api.Features.CreateLinkToken;

public class CreateLinkTokenEndpoint : EndpointWithoutRequest<CreateLinkTokenResponse>
{
    public required CreateLinkTokenCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Get("/link");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var token = await Handler.HandleAsync(new(), ct);
        await SendOkAsync(new CreateLinkTokenResponse
        {
            LinkToken = token
        });
    }
}