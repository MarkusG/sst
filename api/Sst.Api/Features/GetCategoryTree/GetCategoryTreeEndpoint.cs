using FastEndpoints;
using Sst.Contracts.Responses;

namespace Sst.Api.Features.GetCategoryTree;

public class GetCategoryTreeEndpoint : Endpoint<CategoryTreeResponse>
{
    public required GetCategoryTreeQuery.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Get("/categories/tree");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CategoryTreeResponse req, CancellationToken ct)
    {
        var response = await Handler.HandleAsync(null, ct);
        await SendOkAsync(response);
    }
}