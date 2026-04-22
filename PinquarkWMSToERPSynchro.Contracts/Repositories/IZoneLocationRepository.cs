using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Contracts.Repositories
{
    public interface IZoneLocationRepository
    {
        Task UpsertAsync(IEnumerable<LocationZone> items, CancellationToken cancellationToken = default);
    }
}
