using System.Globalization;
using CsvHelper;

namespace Sst.Api.Features.Transactions.Import.Mappers;

public class CitibankTransactionMapper : ITransactionMapper
{
    private record Transaction
    {
        public required DateTimeOffset Date { get; init; }

        public required string Description { get; init; }

        public required decimal? Debit { get; init; }

        public required decimal? Credit { get; init; }
    }

    public bool CanMap(string csv)
    {
        using var reader = new StringReader(csv);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

        csvReader.Read();
        csvReader.ReadHeader();

        if (csvReader.HeaderRecord is null)
            return false;

        var fields = new List<string> { "Date", "Description", "Debit", "Credit" };

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
            Amount = -t.Debit ?? -t.Credit ?? 0,
            Currency = "USD",
            Description = t.Description,
            Timestamp = t.Date.ToUniversalTime()
        }).ToList();
    }
}