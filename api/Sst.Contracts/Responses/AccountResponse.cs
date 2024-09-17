namespace Sst.Contracts.Responses;

public class AccountResponse
{
    public required string Name { get; set; }
    
    public required decimal? AvailableBalance { get; set; }
    
    public required decimal? CurrentBalance { get; set; }
}