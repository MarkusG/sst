using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Database;

namespace Sst.Api.Features.UpdateTransaction;

[Handler]
public partial class UpdateTransactionCommand
{
    public record Command
    {
        public int Id { get; set; }
    
        public required DateTimeOffset? Timestamp { get; set; }
    
        public required decimal Amount { get; set; }
    
        public required string Description { get; set; }
    
        public required string Account { get; set; }
    
        public required string? Category { get; set; }
    }
    
    private static async ValueTask<bool> HandleAsync(Command req, SstDbContext ctx, CancellationToken token)
    {
        var affected = await ctx.Transactions
            .Where(t => t.Id == req.Id)
            .ExecuteUpdateAsync(t => t
                .SetProperty(tt => tt.Timestamp, req.Timestamp)
                .SetProperty(tt => tt.Amount, req.Amount)
                .SetProperty(tt => tt.Description, req.Description)
                .SetProperty(tt => tt.AccountMask, req.Account)
                .SetProperty(tt => tt.Category, req.Category));

        return affected > 0;
    }
}