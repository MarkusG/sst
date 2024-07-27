namespace Sst.Api.Features.GetTransactions;

public class GetTransactionsRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 100;
    
    public string? SortField { get; set; }
    
    public string? SortDirection { get; set; }
}