using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.CreateCategory;

public class CreateCategoryEndpoint : Endpoint<CreateCategoryRequest>
{
    public required CreateCategoryCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Post("/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateCategoryRequest req, CancellationToken ct)
    {
        var response = await Handler.HandleAsync(new CreateCategoryCommand.Command
        {
            Name = req.Name
        });

        await SendOkAsync(response);
    }
}