namespace Sst.Contracts.Responses;

public class TransactionsResponse : PaginatedResponse
{
    public required TransactionResponse[] Transactions { get; set; }
}