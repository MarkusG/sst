using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sst.Database;
using Sst.Database.Entities;
using Sst.Plaid;
using Sst.Plaid.Endpoints.SyncTransactions;

namespace Sst.Api.Features.SyncTransactions;

[Handler]
public static partial class SyncTransactionsCommand
{
    public record Command
    {
        public required int ItemId { get; init; }
    }

    private static async ValueTask HandleAsync(
        Command req,
        SstDbContext ctx,
        PlaidClient client,
        IOptions<PlaidClientOptions> options,
        CancellationToken ct)
    {
        var item = await ctx.Items.FirstOrDefaultAsync(i => i.Id == req.ItemId, ct);
        if (item is null)
        {
            // TODO proper error handling
            throw new ApplicationException("Item was not found");
        }

        var cursor = item.NextCursor;
        bool hasMore;
        do
        {
            var response = await client.SyncTransactionsAsync(
                new Plaid.Endpoints.SyncTransactions.SyncTransactionsRequest
                {
                    ClientId = options.Value.ClientId,
                    Secret = options.Value.Secret,
                    AccessToken = item.AccessToken,
                    Cursor = cursor,
                    Options = new SyncTransactionsRequestOptions
                    {
                        IncludeOriginalDescription = true
                    }
                }, ct);
            hasMore = response.HasMore;
            cursor = response.NextCursor;

            var accounts = response.Accounts.ToDictionary(a => a.AccountId, a => a);

            foreach (var t in response.Added)
            {
                var timestamp = t switch
                {
                    { AuthorizedDatetime: { } dt } => dt,
                    { AuthorizedDate: { } d } => new DateTimeOffset(d, TimeOnly.MinValue, TimeSpan.Zero),
                    { Datetime: { } dt } => dt,
                    { Date: var d } => new DateTimeOffset(d, TimeOnly.MinValue, TimeSpan.Zero)
                };

                ctx.Transactions.Add(new Transaction
                {
                    PlaidId = t.TransactionId,
                    AccountName = accounts[t.AccountId].Name,
                    AccountMask = accounts[t.AccountId].Mask,
                    Amount = -(decimal)t.Amount,
                    Currency = t.IsoCurrencyCode ?? t.UnofficialCurrencyCode!,
                    Timestamp = timestamp,
                    Description = t.OriginalDescription ?? t.Name
                });
            }
        } while (hasMore);

        item.NextCursor = cursor;
        await ctx.SaveChangesAsync(ct);
    }
}