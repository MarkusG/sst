namespace Sst.Plaid.Endpoints.ItemPublicTokenExchange;

public record ItemPublicTokenExchangeResponse : PlaidResponse
{
    public required string AccessToken { get; set; }
    
    public required string ItemId { get; set; }
}