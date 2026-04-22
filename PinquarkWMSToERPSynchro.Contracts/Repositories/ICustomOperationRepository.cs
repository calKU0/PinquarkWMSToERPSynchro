using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Contracts.Repositories
{
    public interface ICustomOperationRepository
    {
        Task UpsertAsync(IEnumerable<CustomOperation> items, CancellationToken cancellationToken = default);
        Task<DateTime> GetLastSyncDateAsync(CancellationToken cancellationToken = default);
    }
}
