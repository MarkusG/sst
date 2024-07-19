namespace Sst.Plaid.Endpoints.LinkTokenCreate;

public record LinkTokenCreateRequest : PlaidRequest
{
    public record UserRequest
    {
        public string ClientUserId { get; set; } = "0";
    }

    public string ClientName { get; set; } = "sst";

    public string Language { get; set; } = "en";

    public string[] CountryCodes { get; set; } = ["US"];
    
    public required string[] Products { get; set; }

    public UserRequest User { get; set; } = new();
}
