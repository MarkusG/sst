using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Contracts.Responses;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.GetCashFlow;

[Handler]
public partial class GetCashFlowQuery
{
    public record Command
    {
        public required int Year { get; init; }
    }
    
    private static async ValueTask<CashFlowResponse> HandleAsync(Command req, SstDbContext ctx, CancellationToken token)
    {
        var totals = await ctx.Set<CategoryMonthTotal>()
            .Where(t => t.Year == req.Year)
            .ToListAsync();

        return new CashFlowResponse
        {
            Categories = totals
                .GroupBy(t => t.Category)
                .Select(g => new CategoryCashFlowResponse
            {
                Category = g.Key,
                January = g.FirstOrDefault(t => t.Month == 1)?.Total ?? 0,
                February = g.FirstOrDefault(t => t.Month == 2)?.Total ?? 0,
                March = g.FirstOrDefault(t => t.Month == 3)?.Total ?? 0,
                April = g.FirstOrDefault(t => t.Month == 4)?.Total ?? 0,
                May = g.FirstOrDefault(t => t.Month == 5)?.Total ?? 0,
                June = g.FirstOrDefault(t => t.Month == 6)?.Total ?? 0,
                July = g.FirstOrDefault(t => t.Month == 7)?.Total ?? 0,
                August = g.FirstOrDefault(t => t.Month == 8)?.Total ?? 0,
                September = g.FirstOrDefault(t => t.Month == 9)?.Total ?? 0,
                October = g.FirstOrDefault(t => t.Month == 10)?.Total ?? 0,
                November = g.FirstOrDefault(t => t.Month == 11)?.Total ?? 0,
                December = g.FirstOrDefault(t => t.Month == 12)?.Total ?? 0,
            })
        };
    }
}