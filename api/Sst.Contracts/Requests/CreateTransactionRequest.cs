namespace Sst.Contracts.Requests;

public class CreateTransactionRequest
{
    public required DateTimeOffset Timestamp { get; set; }
    
    public required decimal Amount { get; set; }
    
    public required string Description { get; set; }
    
    public required string Account { get; set; }
    
    public required string? Category { get; set; }
}