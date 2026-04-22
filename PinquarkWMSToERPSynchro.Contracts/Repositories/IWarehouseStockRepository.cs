using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Contracts.Repositories
{
    public interface IWarehouseStockRepository
    {
        Task UpsertAsync(IEnumerable<WarehouseStock> items, CancellationToken cancellationToken = default);
    }
}
