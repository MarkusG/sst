namespace Sst.Api.Features.CreateItem;

public record CreateItemRequest
{
    public required string AccessToken { get; set; }
}