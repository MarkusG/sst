namespace Sst.Contracts.Requests;

public record ExchangePublicTokenRequest
{
    public required string PublicToken { get; set; }
}