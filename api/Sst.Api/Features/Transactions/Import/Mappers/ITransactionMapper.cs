using Sst.Database.Entities;

namespace Sst.Api.Features.Transactions.Import.Mappers;

public interface ITransactionMapper
{
    bool CanMap(string csv);

    List<Transaction> GetTransactions(string csv);
}