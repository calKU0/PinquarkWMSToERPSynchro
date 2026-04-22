using PinquarkWMSToERPSynchro.Contracts.DTOs;
using PinquarkWMSToERPSynchro.Contracts.DTOs.Requests;
using PinquarkWMSToERPSynchro.Contracts.DTOs.Responses;

namespace PinquarkWMSToERPSynchro.Contracts.Clients
{
    public interface IPinquarkApiClient
    {
        Task<List<GetZonesResponse>> GetZonesAsync(string endpoint, CancellationToken cancellationToken = default);
        Task<List<GetLocationsResponse>> GetLocationsAsync(string endpoint, GetLocationsRequest request, CancellationToken cancellationToken = default);
        Task<List<GetWarehouseStockResponse>> GetWarehouseStockAsync(string endpoint, CancellationToken cancellationToken = default);
        Task<List<GetDocsResponse>> GetDocsAsync(string endpoint, CancellationToken cancellationToken = default);
        Task<List<GetTasksResponse>> GetTasksAsync(string endpoint, GetTasksRequest request, CancellationToken cancellationToken = default);
        Task<List<GetLuResourcesResponse>> GetLuResourcesAsync(string endpoint, CancellationToken cancellationToken = default);
        Task<List<GetOperationsResponse>> GetOperationsAsync(string endpoint, GetOperationsRequest request, CancellationToken cancellationToken = default);
        Task<List<GetCustomOperationResponse>> GetCustomOperationsAsync(string endpoint, GetCustomOperetionsRequest request, CancellationToken cancellationToken = default);
    }
}
