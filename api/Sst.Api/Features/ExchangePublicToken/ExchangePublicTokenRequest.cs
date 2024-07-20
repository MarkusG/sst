namespace Sst.Api.Features.ExchangePublicToken;

public record ExchangePublicTokenRequest
{
    public required string PublicToken { get; set; }
}