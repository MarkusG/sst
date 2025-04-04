using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Features.ImportTransactions.Mappers;
using Sst.Database;

namespace Sst.Api.Features.ImportTransactions;

[Handler]
public partial class ImportTransactionsCommand
{
    public record Command
    {
        public required string AccountName { get; set; }

        public required IFormFile File { get; set; }
    }

    private static async ValueTask HandleAsync(Command req, SstDbContext ctx, TransactionMapperProvider mapperProvider, CancellationToken token)
    {
        var reader = new StreamReader(req.File.OpenReadStream());
        var csv = await reader.ReadToEndAsync();

        if (mapperProvider.TryGetMapper(csv, out var mapper))
        {
            var newTransactions = mapper.GetTransactions(csv);
            var earliest = newTransactions.OrderBy(t => t.Timestamp).FirstOrDefault();

            if (earliest is null)
                return;

            var existingTransactions = await ctx.Transactions
                .Where(t => t.Timestamp >= earliest.Timestamp)
                .ToListAsync();

            // deduplicate
            foreach (var t in newTransactions.Where(t =>
                         !existingTransactions.Any(tt =>
                             tt.AccountName == req.AccountName
                             && tt.Timestamp == t.Timestamp
                             && tt.Description == t.Description
                             && tt.Amount == t.Amount)))
            {
                t.AccountName = req.AccountName;
                ctx.Transactions.Add(t);
            }
        }

        await ctx.SaveChangesAsync(token);
    }
}