namespace Sst.Contracts.Responses;

public class CategoryTreeResponse
{
    public required IEnumerable<CategoryTreeEntryResponse> Categories { get; set; }
}