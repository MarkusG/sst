namespace Sst.Contracts.Responses;

public class CategoryTreeEntryResponse
{
    public required int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required int Position { get; set; }
    
    public required int? ParentId { get; set; }
    
    public required IEnumerable<CategoryTreeEntryResponse> Subcategories { get; set; }
}