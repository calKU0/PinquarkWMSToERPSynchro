using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Contracts.Repositories
{
    public interface ILogisticUnitRepository
    {
        Task UpsertAsync(IEnumerable<LogisticUnit> items, CancellationToken cancellationToken = default);
    }
}
