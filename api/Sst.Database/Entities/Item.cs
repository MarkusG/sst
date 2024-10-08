namespace Sst.Database.Entities;

public class Item
{
    public int Id { get; set; }
    
    public required string AccessToken { get; set; }
    
    public required string? NextCursor { get; set; }

    public List<Account> Accounts { get; set; } = [];
}