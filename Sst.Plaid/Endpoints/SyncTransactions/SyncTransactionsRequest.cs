namespace Sst.Plaid.Endpoints.SyncTransactions;

public record SyncTransactionsRequest : PlaidRequest
{
    public required SyncTransactionsRequestOptions Options { get; set; }
    
    public required string AccessToken { get; set; }
    
    public required string? Cursor { get; set; }
}