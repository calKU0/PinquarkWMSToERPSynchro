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
        private readonly IReadOnlyDictionary<string, SyncEndpoint> _endpointsByName;

        public Worker(
            ILogger<Worker> logger,
            IServiceProvider serviceProvider,
            IOptions<PinquarkApiSettings> apiOptions)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _apiSettings = apiOptions.Value;

            _endpointsByName = _apiSettings.Endpoints
                .ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker Service started at: {time}", DateTimeOffset.Now);

            // We only start loops for endpoints. 
            // Dependencies are handled inside each loop iteration to ensure freshness.
            var syncTasks = _apiSettings.Endpoints.Select(endpoint =>
                RunEndpointLoopAsync(endpoint, stoppingToken)
            );

            await Task.WhenAll(syncTasks);
        }

        private async Task RunEndpointLoopAsync(SyncEndpoint endpoint, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting loop for {Name} (Interval: {Sec}s)", endpoint.Name, endpoint.SyncIntervalSeconds);

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(endpoint.SyncIntervalSeconds));

            do
            {
                // Each tick performs the full chain: Dependencies -> Main Endpoint
                await SyncChainAsync(endpoint, stoppingToken);

            } while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken));
        }

        private async Task SyncChainAsync(SyncEndpoint endpoint, CancellationToken stoppingToken)
        {
            // We use a Local HashSet to prevent circular infinite loops within a single chain
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            await ExecuteStepAsync(endpoint, visited, stoppingToken);
        }

        private async Task ExecuteStepAsync(SyncEndpoint endpoint, HashSet<string> visited, CancellationToken stoppingToken)
        {
            if (!visited.Add(endpoint.Name)) return;

            // 1. Sync all dependencies FIRST to ensure they are fresh for the current main sync
            if (endpoint.DependsOn != null)
            {
                foreach (var depName in endpoint.DependsOn)
                {
                    if (_endpointsByName.TryGetValue(depName, out var depEndpoint))
                    {
                        _logger.LogInformation("Syncing dependency {Dep} for {Main}", depName, endpoint.Name);
                        await ExecuteStepAsync(depEndpoint, visited, stoppingToken);
                    }
                }
            }

            // 2. Now sync the actual endpoint
            await PerformSyncActionAsync(endpoint, stoppingToken);
        }

        private async Task PerformSyncActionAsync(SyncEndpoint endpoint, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();

                _logger.LogDebug("Executing sync for {EndpointName}", endpoint.Name);
                await syncService.SyncAsync(endpoint.Name, endpoint.Endpoint, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing {EndpointName}", endpoint.Name);
            }
        }
    }
}