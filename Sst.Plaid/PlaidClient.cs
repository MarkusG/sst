using System.Net.Http.Json;
using System.Text.Json;
using Sst.Plaid.Endpoints.ItemPublicTokenExchange;
using Sst.Plaid.Endpoints.LinkTokenCreate;
using Sst.Plaid.Endpoints.SyncTransactions;

namespace Sst.Plaid;

public class PlaidClient(HttpClient httpClient)
{
    private async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
    {
        var rawResponse = await httpClient.PostAsJsonAsync(endpoint, request, PlaidJsonOptions.Options);
        try
        {
            rawResponse.EnsureSuccessStatusCode();
        }
        catch
        {
            Console.WriteLine(await rawResponse.Content.ReadAsStringAsync());
            throw;
        }

        var responseStream = await rawResponse.Content.ReadAsStreamAsync();
        var response = await JsonSerializer.DeserializeAsync<TResponse>(responseStream, PlaidJsonOptions.Options);
        if (response is null)
            throw new ApplicationException("Failed to deserialize the response from Plaid");

        return response;
    }

    public async Task<SyncTransactionsResponse> SyncTransactionsAsync(SyncTransactionsRequest request) =>
        await PostAsync<SyncTransactionsRequest, SyncTransactionsResponse>("transactions/sync", request);

    public async Task<LinkTokenCreateResponse> LinkTokenCreate(LinkTokenCreateRequest request) =>
        await PostAsync<LinkTokenCreateRequest, LinkTokenCreateResponse>("link/token/create", request);
    
    public async Task<ItemPublicTokenExchangeResponse> ItemPublicTokenExchange(ItemPublicTokenExchangeRequest request) =>
        await PostAsync<ItemPublicTokenExchangeRequest, ItemPublicTokenExchangeResponse>("item/public_token/exchange", request);
}