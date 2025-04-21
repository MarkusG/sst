using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Immediate.Validations.Shared;
using Microsoft.EntityFrameworkCore;
using Sst.Api.Common;
using Sst.Database;
using Sst.Database.Entities;

namespace Sst.Api.Features.GetTransactions;

[Handler]
[MapGet("/transactions")]
public static partial class GetAll
{
    [Validate]
    public sealed partial record Query : IValidationTarget<Query>
    {
        public required int Page { get; init; }

        [GreaterThan(0)]
        public required int PageSize { get; init; }

        [OneOf("timestamp", "description", "account", "amount", null!)]
        public required string? SortField { get; init; }

        [OneOf("up", "down", null!)]
        public required string? SortDirection { get; init; }

        public required DateTimeOffset? From { get; init; }

        public required DateTimeOffset? To { get; init; }

        public required int? Offset { get; init; }
    }

    public sealed record Response : PaginatedResponse
    {
        public sealed record Transaction
        {
            public sealed record Categorization
            {
                public required int Id { get; init; }

                public required string Category { get; init; }

                public required decimal Amount { get; init; }

                public required int Position { get; init; }
            }

            public required int Id { get; init; }

            public required DateTimeOffset Timestamp { get; init; }

            public required string Account { get; init; }

            public required string Description { get; init; }

            public required decimal Amount { get; init; }

            public required IEnumerable<Categorization> Categorizations { get; init; }
        }

        public required IEnumerable<Transaction> Transactions { get; init; }
    }

    private static async ValueTask<Response> HandleAsync(
        Query request,
        SstDbContext ctx,
        CancellationToken token)
    {
        IQueryable<Transaction> query = ctx.Transactions
            .Include(t => t.Categorizations.OrderBy(cz => cz.Position))
            .ThenInclude(c => c.Category)
            .Include(t => t.Account);

        if (request.From is { } from)
        {
            var fromOffset = from.AddMinutes(request.Offset!.Value);
            query = query.Where(t => t.Timestamp >= fromOffset);
        }

        if (request.To is { } to)
        {
            var toOffset = to.AddMinutes(request.Offset!.Value);
            query = query.Where(t => t.Timestamp <= toOffset);
        }

        IOrderedQueryable<Transaction> sortedQuery;
        if (request is { SortField: not null, SortDirection: "up" })
        {
            sortedQuery = request.SortField switch
            {
                "timestamp" => query.OrderByDescending(t => t.Timestamp),
                "amount" => query.OrderByDescending(t => t.Categorizations.Select(cz => (int?)cz.Amount).Max() ?? t.Amount),
                "account" => query.OrderByDescending(t => t.Account!.Name),
                "description" => query.OrderByDescending(t => t.Description),
                _ => throw new ArgumentOutOfRangeException(nameof(request.SortField))
            };
        }
        else if (request.SortField is not null)
        {
            sortedQuery = request.SortField switch
            {
                "timestamp" => query.OrderBy(t => t.Timestamp),
                "amount" => query.OrderBy(t => t.Categorizations.Select(cz => (int?)cz.Amount).Max() ?? t.Amount),
                "account" => query.OrderBy(t => t.Account!.Name),
                "description" => query.OrderBy(t => t.Description),
                _ => throw new ArgumentOutOfRangeException(nameof(request.SortField))
            };
        }
        else
        {
            sortedQuery = query.OrderByDescending(t => t.Timestamp);
        }

        var transactions = await sortedQuery
            .ThenBy(t => t.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(token);

        var totalCount = await ctx.Transactions.CountAsync(token);

        return new Response
        {
            Page = request.Page,
            PageCount = transactions.Count,
            TotalCount = totalCount,
            TotalPages = totalCount / request.PageSize + 1,
            Transactions = transactions.Select(t => new Response.Transaction
            {
                Id = t.Id,
                Timestamp = t.Timestamp!.Value,
                Account = t.Account!.Name,
                Amount = t.Amount,
                Description = t.Description,
                Categorizations = t.Categorizations.Select(cz => new Response.Transaction.Categorization
                {
                    Id = cz.Id,
                    Amount = cz.Amount,
                    Category = cz.Category!.Name,
                    Position = cz.Position
                })
            })
        };
    }
}