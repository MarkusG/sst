namespace Sst.Plaid.Endpoints.SyncTransactions;

public class SyncTransactionsRequest : PlaidRequest
{
    public required SyncTransactionsRequestOptions Options { get; set; }
    
    public required string? Cursor { get; set; }
}