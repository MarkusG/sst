namespace Sst.Contracts.Responses;

public class CashFlowTreeEntryResponse
{
    public required int Id { get; set; }
    
    public required string Category { get; set; }
    
    public required CashFlowTreeTotalResponse January { get; set; }
    
    public required CashFlowTreeTotalResponse February { get; set; }
    
    public required CashFlowTreeTotalResponse March { get; set; }
    
    public required CashFlowTreeTotalResponse April { get; set; }
    
    public required CashFlowTreeTotalResponse May { get; set; }
    
    public required CashFlowTreeTotalResponse June { get; set; }
    
    public required CashFlowTreeTotalResponse July { get; set; }
    
    public required CashFlowTreeTotalResponse August { get; set; }
    
    public required CashFlowTreeTotalResponse September { get; set; }
    
    public required CashFlowTreeTotalResponse October { get; set; }
    
    public required CashFlowTreeTotalResponse November { get; set; }
    
    public required CashFlowTreeTotalResponse December { get; set; }
    
    public required decimal TreeTotal { get; set; }
    
    public required decimal CategoryTotal { get; set; }
    
    public required IEnumerable<CashFlowTreeEntryResponse> Children { get; set; }
}