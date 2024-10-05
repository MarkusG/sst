namespace Sst.Contracts.Requests;

public class UpdateCategoryRequest
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required int Position { get; set; }
    
    public required int? SuperCategoryId { get; set; }
}