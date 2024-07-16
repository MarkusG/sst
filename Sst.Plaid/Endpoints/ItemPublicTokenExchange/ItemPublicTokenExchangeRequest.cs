namespace Sst.Plaid.Endpoints.ItemPublicTokenExchange;

public record ItemPublicTokenExchangeRequest : PlaidRequest
{
    public required string PublicToken { get; set; }
}