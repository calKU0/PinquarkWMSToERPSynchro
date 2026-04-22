using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Contracts.Repositories
{
    public interface ITaskRepository
    {
        Task UpsertAsync(IEnumerable<TaskModel> items, CancellationToken cancellationToken = default);
    }
}
