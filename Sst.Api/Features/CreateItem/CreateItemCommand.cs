using Immediate.Handlers.Shared;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.CreateItem;

[Handler]
public static partial class CreateItemCommand
{
    public record Command
    {
        public required string AccessToken { get; set; }
    }

    private static async ValueTask<int> HandleAsync(Command req, SstDbContext ctx, CancellationToken token)
    {
        var account = ctx.Items.Add(new Item
        {
            AccessToken = req.AccessToken
        }).Entity;

        await ctx.SaveChangesAsync(token);
        // TODO error handling (conflict)
        return account.Id;
    }
}