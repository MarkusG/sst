namespace Sst.Plaid.Endpoints.SyncTransactions;

public record SyncTransactionsRequestOptions
{
    public required bool IncludeOriginalDescription { get; set; }
}