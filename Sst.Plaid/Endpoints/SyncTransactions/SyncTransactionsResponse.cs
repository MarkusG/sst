namespace Sst.Plaid.Endpoints.SyncTransactions;

public record SyncTransactionsResponse
{
    public required AccountResponse[] Accounts { get; set; }
    public required TransactionResponse[] Added { get; set; }
    public required TransactionResponse[] Modified { get; set; }
    public required RemovedTransactionResponse[] Removed { get; set; }
    public required string NextCursor { get; set; }
    public required bool HasMore { get; set; }
    public required string RequestId { get; set; }
    public required string TransactionsUpdateStatus { get; set; }
}

public record AccountResponse
{
    public required string AccountId { get; set; }
    public required BalancesResponse Balances { get; set; }
    public required string? Mask { get; set; }
    public required string Name { get; set; }
    public required string? OfficialName { get; set; }
    public required string? Subtype { get; set; }
    public required string Type { get; set; }
}

public record BalancesResponse
{
    public required double? Available { get; set; }
    public required double? Current { get; set; }
    public required string? IsoCurrencyCode { get; set; }
    public required double? Limit { get; set; }
    public required string? UnofficialCurrencyCode { get; set; }
}

public record TransactionResponse
{
    public required string AccountId { get; set; }
    public required string? AccountOwner { get; set; }
    public required double Amount { get; set; }
    public required string? IsoCurrencyCode { get; set; }
    public required string? UnofficialCurrencyCode { get; set; }
    public required string? CheckNumber { get; set; }
    public required CounterpartyResponse[] Counterparties { get; set; }
    public required DateOnly Date { get; set; }
    public required DateTimeOffset? Datetime { get; set; }
    public required DateOnly? AuthorizedDate { get; set; }
    public required DateTimeOffset? AuthorizedDatetime { get; set; }
    public required Location Location { get; set; }
    public required string Name { get; set; }
    public required string? MerchantName { get; set; }
    public required string? MerchantEntityId { get; set; }
    public required string? LogoUrl { get; set; }
    public required string? Website { get; set; }
    public required PaymentMeta PaymentMeta { get; set; }
    public required string PaymentChannel { get; set; }
    public required bool Pending { get; set; }
    public required string? PendingTransactionId { get; set; }
    public required PersonalFinanceCategory? PersonalFinanceCategory { get; set; }
    public required string? PersonalFinanceCategoryIconUrl { get; set; }
    public required string TransactionId { get; set; }
    public required string? TransactionCode { get; set; }
    public string? OriginalDescription { get; set; }
}

public record CounterpartyResponse
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string? LogoUrl { get; set; }
    public required string? Website { get; set; }
    public required string? EntityId { get; set; }
    public required string? ConfidenceLevel { get; set; }
}

public record Location
{
    public required string? Address { get; set; }
    public required string? City { get; set; }
    public required string? Region { get; set; }
    public required string? PostalCode { get; set; }
    public required string? Country { get; set; }
    public required double? Lat { get; set; }
    public required double? Lon { get; set; }
    public required string? StoreNumber { get; set; }
}

public record PaymentMeta
{
    public required string? ByOrderOf { get; set; }
    public required string? Payee { get; set; }
    public required string? Payer { get; set; }
    public required string? PaymentMethod { get; set; }
    public required string? PaymentProcessor { get; set; }
    public required string? PpdId { get; set; }
    public required string? Reason { get; set; }
    public required string? ReferenceNumber { get; set; }
}

public record PersonalFinanceCategory
{
    public required string Primary { get; set; }
    public required string Detailed { get; set; }
    public required string? ConfidenceLevel { get; set; }
}

public record RemovedTransactionResponse
{
    public required string AccountId { get; set; }
    public required string TransactionId { get; set; }
}