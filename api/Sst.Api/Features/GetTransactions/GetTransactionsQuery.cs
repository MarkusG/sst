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
        IOrderedQueryable<Transaction> query;
        if (request is { SortField: not null, SortDirection: "up" })
        {
            query = request.SortField switch
            {
                "timestamp" => ctx.Transactions.OrderByDescending(t => t.Timestamp),
                "amount" => ctx.Transactions.OrderByDescending(t => t.Amount),
                "description" => ctx.Transactions.OrderByDescending(t => t.Description),
                "account" => ctx.Transactions.OrderByDescending(t => t.AccountName),
                "category" => ctx.Transactions.OrderByDescending(t => t.Category),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else if (request.SortField is not null)
        {
            query = request.SortField switch
            {
                "timestamp" => ctx.Transactions.OrderBy(t => t.Timestamp),
                "amount" => ctx.Transactions.OrderBy(t => t.Amount),
                "description" => ctx.Transactions.OrderBy(t => t.Description),
                "account" => ctx.Transactions.OrderBy(t => t.AccountName),
                "category" => ctx.Transactions.OrderBy(t => t.Category),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else
        {
            query = ctx.Transactions.OrderByDescending(t => t.Timestamp);
        }

        var transactions = await query
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
                Category = t.Category
            })
        };
    }
}