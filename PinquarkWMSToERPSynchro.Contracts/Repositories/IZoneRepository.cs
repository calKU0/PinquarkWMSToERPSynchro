using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Contracts.Repositories
{
    public interface IZoneRepository
    {
        Task UpsertAsync(IEnumerable<Zone> items, CancellationToken cancellationToken = default);
    }
}
