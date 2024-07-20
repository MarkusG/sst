namespace Sst.Plaid;

public abstract record PlaidResponse
{
    public required string RequestId { get; set; }
}