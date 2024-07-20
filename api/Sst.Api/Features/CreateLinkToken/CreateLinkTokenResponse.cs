namespace Sst.Api.Features.CreateLinkToken;

public record CreateLinkTokenResponse
{
    public required string LinkToken { get; set; }
}