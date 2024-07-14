using System.Net.Http.Json;
using System.Text.Json;
using Sst.Plaid.Endpoints.SyncTransactions;

namespace Sst.Plaid;

public class PlaidClient(HttpClient httpClient)
{
    public async Task<SyncTransactionsResponse> SyncTransactionsAsync(SyncTransactionsRequest request)
    {
        var rawResponse = await httpClient.PostAsJsonAsync("transactions/sync", request, PlaidJsonOptions.Options);
        rawResponse.EnsureSuccessStatusCode();

        var responseStream = await rawResponse.Content.ReadAsStreamAsync();
        var response = await JsonSerializer.DeserializeAsync<SyncTransactionsResponse>(responseStream, PlaidJsonOptions.Options);
        if (response is null)
            throw new ApplicationException("Failed to deserialize the response from Plaid");
        
        return response;
    }
}