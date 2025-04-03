namespace Sst.Contracts.Requests;

public class CategorizeTransactionRequest
{
    public int TransactionId { get; set; }
    
    public required decimal Amount { get; set; }

    public string Category { get; set; } = string.Empty;
    
    public required int Position { get; set; }
}