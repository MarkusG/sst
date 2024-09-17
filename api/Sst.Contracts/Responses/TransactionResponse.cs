namespace Sst.Contracts.Responses;

public class TransactionResponse
{
    public required int Id { get; set; }
    
    public required DateTimeOffset? Timestamp { get; set; }
    
    public required decimal Amount { get; set; }
    
    public required string Description { get; set; }
    
    public required string Account { get; set; }
    
    public required string? Category { get; set; }
}