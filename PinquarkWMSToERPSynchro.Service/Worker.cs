using Microsoft.Extensions.Options;
using PinquarkWMSToERPSynchro.Contracts.Settings;
using PinquarkWMSToERPSynchro.Service.Services;

namespace PinquarkWMSToERPSynchro.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly PinquarkApiSettings _apiSettings;

        public Worker(
            ILogger<Worker> logger,
            IServiceProvider serviceProvider,
            IOptions<PinquarkApiSettings> apiOptions)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _apiSettings = apiOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker Service started at: {time}", DateTimeOffset.Now);

            var endpointsByName = _apiSettings.Endpoints
                .ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);

            var syncTasks = _apiSettings.Endpoints.Select(endpoint =>
                RunEndpointSyncAsync(endpoint, endpointsByName, stoppingToken)
            );

            await Task.WhenAll(syncTasks);
        }

        private async Task RunEndpointSyncAsync(
            SyncEndpoint endpoint,
            IReadOnlyDictionary<string, SyncEndpoint> endpointsByName,
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Starting sync loop for {EndpointName} with interval {Interval} minutes",
                endpoint.Name,
                endpoint.SyncIntervalMinutes);

            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(endpoint.SyncIntervalMinutes));

            await ExecuteSyncWithDependenciesAsync(endpoint, endpointsByName, stoppingToken);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                await ExecuteSyncWithDependenciesAsync(endpoint, endpointsByName, stoppingToken);
            }
        }

        private async Task ExecuteSyncWithDependenciesAsync(
            SyncEndpoint endpoint,
            IReadOnlyDictionary<string, SyncEndpoint> endpointsByName,
            CancellationToken stoppingToken)
        {
            var executing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            await ExecuteEndpointAndDependenciesAsync(endpoint, endpointsByName, executing, stoppingToken);
        }

        private async Task ExecuteEndpointAndDependenciesAsync(
            SyncEndpoint endpoint,
            IReadOnlyDictionary<string, SyncEndpoint> endpointsByName,
            HashSet<string> executing,
            CancellationToken stoppingToken)
        {
            if (!executing.Add(endpoint.Name))
            {
                _logger.LogWarning("Circular dependency detected for endpoint {EndpointName}", endpoint.Name);
                return;
            }

            try
            {
                foreach (var dependencyName in endpoint.DependsOn)
                {
                    if (!endpointsByName.TryGetValue(dependencyName, out var dependencyEndpoint))
                    {
                        _logger.LogWarning(
                            "Dependency endpoint {DependencyName} configured for {EndpointName} was not found",
                            dependencyName,
                            endpoint.Name);
                        continue;
                    }

                    await ExecuteEndpointAndDependenciesAsync(
                        dependencyEndpoint,
                        endpointsByName,
                        executing,
                        stoppingToken);
                }

                await ExecuteSyncAsync(endpoint, stoppingToken);
            }
            finally
            {
                executing.Remove(endpoint.Name);
            }
        }

        private async Task ExecuteSyncAsync(SyncEndpoint endpoint, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();

                await syncService.SyncAsync(endpoint.Name, endpoint.Endpoint, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing sync for {EndpointName}", endpoint.Name);
            }
        }
    }
}
