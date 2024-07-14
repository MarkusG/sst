namespace Sst.Plaid;

public abstract class PlaidRequest
{
    public required string ClientId { get; set; }
    
    public required string Secret { get; set; }
    
    public required string AccessToken { get; set; }
}