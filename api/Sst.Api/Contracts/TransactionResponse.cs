namespace Sst.Api.Contracts;

public class TransactionResponse
{
    public required DateTimeOffset? Timestamp { get; set; }
    
    public required decimal Amount { get; set; }
    
    public required string Description { get; set; }
    
    public required string Account { get; set; }
    
    public required string? Category { get; set; }
}