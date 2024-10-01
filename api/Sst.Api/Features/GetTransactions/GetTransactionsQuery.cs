using Immediate.Handlers.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Contracts;
using Sst.Contracts.Responses;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.GetTransactions;

[Handler]
public partial class GetTransactionsQuery
{
    public record Query
    {
        public required int Page { get; set; }
        
        public required int PageSize { get; set; }
        
        public required string? SortField { get; set; }
        
        public required string? SortDirection { get; set; }
    }

    private static async ValueTask<TransactionsResponse> HandleAsync(
        Query request,
        SstDbContext ctx,
        CancellationToken token)
    {
        var query = ctx.Transactions.Include(t => t.Category);
        IOrderedQueryable<Transaction> sortedQuery;
        if (request is { SortField: not null, SortDirection: "up" })
        {
            sortedQuery = request.SortField switch
            {
                "timestamp" => query.OrderByDescending(t => t.Timestamp),
                "amount" => query.OrderByDescending(t => t.Amount),
                "description" => query.OrderByDescending(t => t.Description),
                "account" => query.OrderByDescending(t => t.AccountName),
                "category" => query.OrderByDescending(t => t.Category),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else if (request.SortField is not null)
        {
            sortedQuery = request.SortField switch
            {
                "timestamp" => query.OrderBy(t => t.Timestamp),
                "amount" => query.OrderBy(t => t.Amount),
                "description" => query.OrderBy(t => t.Description),
                "account" => query.OrderBy(t => t.AccountName),
                "category" => query.OrderBy(t => t.Category),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else
        {
            sortedQuery = ctx.Transactions.OrderByDescending(t => t.Timestamp);
        }

        var transactions = await sortedQuery
            .ThenBy(t => t.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(token);

        var totalCount = await ctx.Transactions.CountAsync(token);

        return new TransactionsResponse
        {
            Page = request.Page,
            PageCount = transactions.Count,
            TotalCount = totalCount,
            TotalPages = totalCount / request.PageSize + 1,
            Transactions = transactions.Select(t => new TransactionResponse
            {
                Id = t.Id,
                Timestamp = t.Timestamp,
                Account = t.AccountName,
                Amount = t.Amount,
                Description = t.Description,
                Category = t.Category?.Name
            })
        };
    }
}