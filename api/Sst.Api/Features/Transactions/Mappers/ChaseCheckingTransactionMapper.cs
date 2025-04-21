using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Sst.Api.Features.Transactions.Mappers;

public class ChaseCheckingTransactionMapper : ITransactionMapper
{
    private record Transaction
    {
        [Name("Posting Date")]
        public required DateTimeOffset Date { get; init; }

        public required string Description { get; init; }

        public required decimal Amount { get; init; }
    }

    public bool CanMap(string csv)
    {
        using var reader = new StringReader(csv);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

        csvReader.Read();
        csvReader.ReadHeader();

        if (csvReader.HeaderRecord is null)
            return false;

        var fields = new List<string> { "Posting Date", "Description", "Amount" };

        return fields.All(f => csvReader.HeaderRecord.Contains(f));
    }

    public List<Database.Entities.Transaction> GetTransactions(string csv)
    {
        using var reader = new StringReader(csv);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csvReader.GetRecords<Transaction>();

        return records.Select(t => new Database.Entities.Transaction
        {
            PlaidId = null,
            AccountId = null,
            Amount = t.Amount,
            Currency = "USD",
            Description = t.Description,
            Timestamp = t.Date.ToUniversalTime()
        }).ToList();
    }
}