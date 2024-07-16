namespace Sst.Plaid;

public abstract record PlaidRequest
{
    public required string ClientId { get; set; }
    
    public required string Secret { get; set; }
}