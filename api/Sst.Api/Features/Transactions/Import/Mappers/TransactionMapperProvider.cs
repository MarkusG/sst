using System.Reflection;

namespace Sst.Api.Features.Transactions.Import.Mappers;

public class TransactionMapperProvider
{
    private readonly List<ITransactionMapper> _mappers;

    public TransactionMapperProvider()
    {
        var mappers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetInterface(nameof(ITransactionMapper)) is not null);

        _mappers = mappers.Select(t => (ITransactionMapper)Activator.CreateInstance(t)!).ToList();
    }

    public bool TryGetMapper(string csv, out ITransactionMapper mapper)
    {
        var m = _mappers.FirstOrDefault(m => m.CanMap(csv));
        mapper = m!;
        return m is not null;
    }
}