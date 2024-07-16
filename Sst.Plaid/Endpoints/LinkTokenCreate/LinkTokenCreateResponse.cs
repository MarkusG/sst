namespace Sst.Plaid.Endpoints.LinkTokenCreate;

public record LinkTokenCreateResponse : PlaidResponse
{
    public required string LinkToken { get; set; }
    
    public required DateTimeOffset Expiration { get; set; }
}