namespace Sst.Contracts.Responses;

public class CashFlowTreeTotalsResponse
{
    public required IEnumerable<decimal> Totals { get; set; }
    
    public required decimal YearTotal { get; set; }
}