using FastEndpoints;
using FluentValidation;
using Sst.Contracts.Requests;

namespace Sst.Api.Features.GetTransactions;

public class GetTransactionsEndpoint : Endpoint<GetTransactionsRequest>
{
    public required GetTransactionsQuery.Handler Handler { get; set; }

    public override void Configure()
    {
        Get("/transactions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTransactionsRequest req, CancellationToken ct)
    {
        var response = await Handler.HandleAsync(new GetTransactionsQuery.Query
        {
            Page = req.Page,
            PageSize = req.PageSize,
            SortDirection = req.SortDirection,
            SortField = req.SortField
        });

        await SendOkAsync(response);
    }
}

public class GetTransactionsRequestValidator : Validator<GetTransactionsRequest>
{
    public GetTransactionsRequestValidator()
    {
        RuleFor(r => r.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be positive");

        RuleFor(r => r.SortDirection)
            .Must(r => r is "up" or "down" or null)
            .WithMessage("Sort direction must be 'up' or 'down'");

        RuleFor(r => r.SortField)
            .Must(r => r is "timestamp" or "amount" or "description" or "account" or "category" or null)
            .WithMessage("Sort field must be one of 'timestamp', 'amount', 'description', 'account', or 'category'");
    }
}