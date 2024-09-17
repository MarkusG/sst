using FastEndpoints;
using Sst.Contracts.Responses;

namespace Sst.Api.Features.GetCategories;

public class GetCategoriesEndpoint : EndpointWithoutRequest<CategoriesResponse>
{
    public required GetCategoriesQuery.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Get("/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await Handler.HandleAsync(null);
        await SendOkAsync(response);
    }
}