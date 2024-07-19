namespace Sst.Plaid.Contracts.AccountsBalanceGet;

public class AccountsBalanceGetResponse
{
    public required AccountResponse[] Accounts { get; set; }
}

public class AccountResponse
{
    public required string AccountId { get; set; }
    public required BalancesResponse Balances { get; set; }
    public string? HolderCategory { get; set; }
    public required string? Mask { get; set; }
    public required string Name { get; set; }
    public required string? OfficialName { get; set; }
    public string? PersistentAccountId { get; set; }
    public required string? Subtype { get; set; }
    public required string Type { get; set; }
}

public class BalancesResponse
{
    public required decimal? Available { get; set; }
    public required decimal? Current { get; set; }
    public required string? IsoCurrencyCode { get; set; }
    public required decimal? Limit { get; set; }
    public required string? UnofficialCurrencyCode { get; set; }
}