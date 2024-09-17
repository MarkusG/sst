namespace Sst.Contracts.Responses;

public class TransactionsResponse : PaginatedResponse
{
    public required IEnumerable<TransactionResponse> Transactions { get; set; }
}