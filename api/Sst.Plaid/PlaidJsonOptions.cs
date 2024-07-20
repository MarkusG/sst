using System.Text.Json;

namespace Sst.Plaid;

public static class PlaidJsonOptions
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
}