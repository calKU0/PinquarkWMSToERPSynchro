using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Contracts.Repositories
{
    public interface IOperationRepository
    {
        Task UpsertAsync(IEnumerable<Operations> items, CancellationToken cancellationToken = default);
        Task<DateTime> GetLastSyncDateAsync(CancellationToken cancellationToken = default);
    }
}
