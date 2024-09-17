namespace Sst.Contracts.Responses;

public abstract class PaginatedResponse
{
    public required int Page { get; set; }
    
    public required int PageCount { get; set; }
    
    public required int TotalPages { get; set; }
    
    public required int TotalCount { get; set; }
}