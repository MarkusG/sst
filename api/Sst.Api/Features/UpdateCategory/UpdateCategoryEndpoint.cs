using FastEndpoints;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.UpdateCategory;

public class UpdateCategoryEndpoint : Endpoint<UpdateCategoryRequest>
{
    public required UpdateCategoryCommand.Handler Handler { get; set; }
    
    public override void Configure()
    {
        Put("/categories/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCategoryRequest req, CancellationToken ct)
    {
        var result = await Handler.HandleAsync(new UpdateCategoryCommand.Command
        {
            Id = req.Id,
            Name = req.Name,
            Position = req.Position,
            SuperCategoryId = req.SuperCategoryId
        });

        if (!result)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendNoContentAsync();
    }
}