namespace Sst.Contracts.Responses;

public class CategoryTreeEntryResponse
{
    public required string Name { get; set; }
    
    public required IEnumerable<CategoryTreeEntryResponse> Subcategories { get; set; }
}