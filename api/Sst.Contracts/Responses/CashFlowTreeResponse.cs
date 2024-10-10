namespace Sst.Contracts.Responses;

public class CashFlowTreeResponse
{
    public required IEnumerable<CashFlowTreeEntryResponse> Categories { get; set; }
    
    public required CashFlowTreeTotalsResponse Totals { get; set; }
}