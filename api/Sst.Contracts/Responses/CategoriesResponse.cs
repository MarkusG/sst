namespace Sst.Contracts.Responses;

public class CategoriesResponse
{
    public required IEnumerable<string> Categories { get; set; }
}