using Sst.Api.Contracts;

namespace Sst.Api.Features.GetTransactions;

public class GetTransactionsResponse
{
    public required int Page { get; set; }
    
    public required int PageCount { get; set; }
    
    public required int TotalPages { get; set; }
    
    public required int TotalCount { get; set; }
    
    public required TransactionResponse[] Transactions { get; set; }
}