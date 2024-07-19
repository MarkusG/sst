namespace Sst.Plaid.Endpoints.AccountsBalanceGet;

public record AccountsBalanceGetRequest : PlaidRequest
{
    public required string AccessToken { get; set; }
}