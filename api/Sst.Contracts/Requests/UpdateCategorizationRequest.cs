namespace Sst.Contracts.Requests;

public class UpdateCategorizationRequest
{
    public int Id { get; set; }
    
    public required string Category { get; set; }
}