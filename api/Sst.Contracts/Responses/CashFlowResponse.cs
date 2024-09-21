namespace Sst.Contracts.Responses;

public class CashFlowResponse
{
    public required IEnumerable<CategoryCashFlowResponse> Categories { get; set; }
    
    public required CashFlowTotalsResponse Totals { get; set; }
}