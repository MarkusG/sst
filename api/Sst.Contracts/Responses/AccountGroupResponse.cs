namespace Sst.Contracts.Responses;

public class AccountGroupResponse
{
    public required int? ItemId { get; set; }
    
    public required IEnumerable<AccountResponse> Accounts { get; set; }
}