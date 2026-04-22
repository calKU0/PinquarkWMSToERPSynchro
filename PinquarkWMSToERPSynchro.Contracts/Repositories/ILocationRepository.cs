using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Contracts.Repositories
{
    public interface ILocationRepository
    {
        Task UpsertAsync(IEnumerable<Location> items, CancellationToken cancellationToken = default);
    }
}
