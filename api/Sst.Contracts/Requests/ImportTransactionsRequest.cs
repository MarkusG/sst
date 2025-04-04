using Microsoft.AspNetCore.Http;

namespace Sst.Contracts.Requests;

public class ImportTransactionsRequest
{
    public required string AccountName { get; set; }
    
    public required IFormFile File { get; set; }
}