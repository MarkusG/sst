namespace Sst.Contracts.Responses;

public class CashFlowTreeEntryResponse
{
    public required int Id { get; set; }
    
    public required string Category { get; set; }
    
    public required IEnumerable<decimal> TreeTotals { get; set; }
    
    public required IEnumerable<decimal> CategoryTotals { get; set; }
    
    public required decimal YearTreeTotal { get; set; }
    
    public required decimal YearCategoryTotal { get; set; }
    
    public required IEnumerable<CashFlowTreeEntryResponse> Children { get; set; }
}