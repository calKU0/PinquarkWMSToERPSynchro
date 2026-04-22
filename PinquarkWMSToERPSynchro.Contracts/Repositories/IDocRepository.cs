using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Contracts.Repositories
{
    public interface IDocRepository
    {
        Task UpsertAsync(IEnumerable<Document> items, CancellationToken cancellationToken = default);
    }
}
