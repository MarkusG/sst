namespace Sst.Api.Features.UpdateTransaction;

public class UpdateTransactionRequest
{
    public int Id { get; set; }
    
    public required DateTimeOffset? Timestamp { get; set; }
    
    public required decimal Amount { get; set; }
    
    public required string Description { get; set; }
    
    public required string Account { get; set; }
    
    public required string? Category { get; set; }
}