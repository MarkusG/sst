namespace Sst.Contracts.Responses;

public class AccountsResponse
{
    public required IEnumerable<AccountGroupResponse> Groups { get; set; }
}