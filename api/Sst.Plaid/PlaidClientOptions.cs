namespace Sst.Plaid;

public sealed class PlaidClientOptions
{
    public required string ClientId { get; set; }

    public required string Secret { get; set; }

    public required string BaseAddress { get; set; }
}