using FastEndpoints;

namespace Sst.Api.Features.LinkPage;

public class LinkPageEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var file = File.OpenRead("link.html");
        await SendStreamAsync(file, contentType: "text/html");
    }
}