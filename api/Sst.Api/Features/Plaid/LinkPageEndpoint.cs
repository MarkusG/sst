using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Sst.Api.Features.Plaid;

[Handler]
[MapGet("/")]
public static partial class LinkPage
{
    private static ValueTask<FileStreamHttpResult> HandleAsync(object _, CancellationToken token)
    {
        var file = File.OpenRead("link.html");
        return ValueTask.FromResult(TypedResults.Stream(file, contentType: "text/html"));
    }
}