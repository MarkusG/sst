namespace Sst.Api.Common;

public abstract record PaginatedResponse
{
    public required int Page { get; init; }

    public required int PageCount { get; init; }

    public required int TotalPages { get; init; }

    public required int TotalCount { get; init; }
}