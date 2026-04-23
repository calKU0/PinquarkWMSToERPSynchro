using PinquarkWMSToERPSynchro.Contracts.Clients;
using PinquarkWMSToERPSynchro.Contracts.DTOs.Requests;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Mappings;

namespace PinquarkWMSToERPSynchro.Service.Services
{
    public class SyncService : ISyncService
    {
        private readonly IPinquarkApiClient _apiClient;
        private readonly IWarehouseStockRepository _warehouseStockRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IZoneLocationRepository _zoneLocationRepository;
        private readonly IDocRepository _docRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogisticUnitRepository _logisticUnitRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly ICustomOperationRepository _customOperationRepository;
        private readonly ILogger<SyncService> _logger;

        public SyncService(
            IPinquarkApiClient apiClient,
            IWarehouseStockRepository warehouseStockRepository,
            IZoneRepository zoneRepository,
            ILocationRepository locationRepository,
            IZoneLocationRepository zoneLocationRepository,
            IDocRepository docRepository,
            ITaskRepository taskRepository,
            ILogisticUnitRepository logisticUnitRepository,
            IOperationRepository operationRepository,
            ICustomOperationRepository customOperationRepository,
            ILogger<SyncService> logger)
        {
            _apiClient = apiClient;
            _warehouseStockRepository = warehouseStockRepository;
            _zoneRepository = zoneRepository;
            _locationRepository = locationRepository;
            _zoneLocationRepository = zoneLocationRepository;
            _docRepository = docRepository;
            _taskRepository = taskRepository;
            _logisticUnitRepository = logisticUnitRepository;
            _operationRepository = operationRepository;
            _customOperationRepository = customOperationRepository;
            _logger = logger;
        }

        public async Task SyncAsync(string endpointName, string endpointUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting synchronization for endpoint: {EndpointName}", endpointName);

                await (endpointName switch
                {
                    "Zones" => SyncZonesAsync(endpointUrl, cancellationToken),
                    "Locations" => SyncLocationsAsync(endpointUrl, cancellationToken),
                    "WarehouseStock" => SyncWarehouseStockAsync(endpointUrl, cancellationToken),
                    "Docs" => SyncDocsAsync(endpointUrl, cancellationToken),
                    "Tasks" => SyncTasksAsync(endpointUrl, cancellationToken),
                    "LuResources" => SyncLuResourcesAsync(endpointUrl, cancellationToken),
                    "Operations" => SyncOperationsAsync(endpointUrl, cancellationToken),
                    "CustomOperations" => SyncCustomOperationsAsync(endpointUrl, cancellationToken),
                    _ => throw new InvalidOperationException($"Unknown endpoint: {endpointName}")
                });

                _logger.LogInformation("Successfully completed synchronization for endpoint: {EndpointName}", endpointName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during synchronization for endpoint: {EndpointName}", endpointName);
                throw;
            }
        }

        private async Task SyncZonesAsync(string endpoint, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetZonesAsync(endpoint, cancellationToken);

            if (response.Any())
            {
                var zones = ZoneMapper.MapToZone(response);
                await _zoneRepository.UpsertAsync(zones, cancellationToken);
                _logger.LogInformation("Synced {Count} zone records", zones.Count);
            }
        }

        private async Task SyncLocationsAsync(string endpoint, CancellationToken cancellationToken)
        {
            var request = new GetLocationsRequest
            {
                Warehouse = "6"
            };

            var response = await _apiClient.GetLocationsAsync(endpoint, request, cancellationToken);

            if (response.Any())
            {
                var locations = LocationMapper.MapToLocation(response);
                await _locationRepository.UpsertAsync(locations, cancellationToken);
                _logger.LogInformation("Synced {Count} location records", locations.Count);

                var locationZones = LocationMapper.MapToLocationZones(response);

                if (locationZones.Any())
                {
                    await _zoneLocationRepository.UpsertAsync(locationZones, cancellationToken);
                    _logger.LogInformation("Synced {Count} zone-location relation records", locationZones.Count);
                }
            }
        }

        private async Task SyncWarehouseStockAsync(string endpoint, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWarehouseStockAsync(endpoint, cancellationToken);

            if (response.Any())
            {
                var warehouseStocks = WarehouseStockMapper.MapToWarehouseStock(response);
                await _warehouseStockRepository.UpsertAsync(warehouseStocks, cancellationToken);
                _logger.LogInformation("Synced {Count} warehouse stock records", warehouseStocks.Count);
            }
        }

        private async Task SyncDocsAsync(string endpoint, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetDocsAsync(endpoint, cancellationToken);

            if (response.Any())
            {
                var documents = DocumentMapper.MapToDocument(response);
                await _docRepository.UpsertAsync(documents, cancellationToken);
                _logger.LogInformation("Synced {Count} document records", documents.Count);
            }
        }

        private async Task SyncTasksAsync(string endpoint, CancellationToken cancellationToken)
        {
            var request = new GetTasksRequest
            {
                DateFrom = DateTime.Now.AddDays(-7).Date,
                DateTo = DateTime.Now.Date,
                Statuses = new List<int> { 0, 1, 3, 12, 13, 14 }
            };

            _logger.LogInformation("Syncing tasks from {DateFrom} to {DateTo} with statuses: {Statuses}", request.DateFrom, request.DateTo, string.Join(", ", request.Statuses));

            var response = await _apiClient.GetTasksAsync(endpoint, request, cancellationToken);

            if (response.Any())
            {
                var tasks = TaskMapper.MapToTask(response);
                await _taskRepository.UpsertAsync(tasks, cancellationToken);
                _logger.LogInformation("Synced {Count} task records", tasks.Count);
            }
        }

        private async Task SyncLuResourcesAsync(string endpoint, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetLuResourcesAsync(endpoint, cancellationToken);

            if (response.Any())
            {
                var logisticUnits = LogisticUnitMapper.MapToLogisticUnits(response);
                await _logisticUnitRepository.UpsertAsync(logisticUnits, cancellationToken);
                _logger.LogInformation("Synced {Count} logistic unit records", logisticUnits.Count);
            }
        }

        private async Task SyncOperationsAsync(string endpoint, CancellationToken cancellationToken)
        {
            var request = new GetOperationsRequest
            {
                DateFrom = await _operationRepository.GetLastSyncDateAsync(cancellationToken),
                DateTo = DateTime.Now.AddSeconds(-1),
            };

            _logger.LogInformation("Syncing operations from {DateFrom} to {DateTo}", request.DateFrom, request.DateTo);

            var response = await _apiClient.GetOperationsAsync(endpoint, request, cancellationToken);

            if (response.Any())
            {
                var operations = OperationMapper.MapToOperations(response);
                await _operationRepository.UpsertAsync(operations, cancellationToken);
                _logger.LogInformation("Synced {Count} operation records", operations.Count);
            }
            else
            {
                _logger.LogInformation("No operation records to sync");
            }
        }

        private async Task SyncCustomOperationsAsync(string endpoint, CancellationToken cancellationToken)
        {
            var request = new GetCustomOperetionsRequest
            {
                DateFrom = await _customOperationRepository.GetLastSyncDateAsync(cancellationToken),
                DateTo = DateTime.Now,
            };

            _logger.LogInformation("Syncing custom operations from {DateFrom} to {DateTo}", request.DateFrom, request.DateTo);

            var response = await _apiClient.GetCustomOperationsAsync(endpoint, request, cancellationToken);

            if (response.Any())
            {
                var customOperations = CustomOperationMapper.MapToCustomOperation(response);
                await _customOperationRepository.UpsertAsync(customOperations, cancellationToken);
                _logger.LogInformation("Synced {Count} custom operation records", customOperations.Count);
            }
            else
            {
                _logger.LogInformation("No custom operation records to sync");
            }
        }
    }
}
