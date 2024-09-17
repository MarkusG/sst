namespace Sst.Contracts.Requests;

public record CreateItemRequest
{
    public required string AccessToken { get; set; }
}