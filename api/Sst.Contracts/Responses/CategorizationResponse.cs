namespace Sst.Contracts.Responses;

public class CategorizationResponse
{
    public required int Id { get; set; }

    public required string Category { get; set; }

    public required decimal Amount { get; set; }
}