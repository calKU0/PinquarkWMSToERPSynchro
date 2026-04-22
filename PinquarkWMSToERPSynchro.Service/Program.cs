using PinquarkWMSToERPSynchro.Contracts.Clients;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Contracts.Settings;
using PinquarkWMSToERPSynchro.Infrastructure.Clients;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using PinquarkWMSToERPSynchro.Infrastructure.Repositories;
using PinquarkWMSToERPSynchro.Service;
using PinquarkWMSToERPSynchro.Service.Constants;
using PinquarkWMSToERPSynchro.Service.Logging;
using PinquarkWMSToERPSynchro.Service.Services;
using Polly;
using Polly.Extensions.Http;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = ServiceConstants.ServiceName;
    })
    .UseSerilog((hostContext, _, loggerConfiguration) =>
    {
        loggerConfiguration.ConfigureServiceLogging(hostContext.Configuration);
    })
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        // Configuration
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.Configure<PinquarkApiSettings>(configuration.GetSection("PinquarkApiSettings"));

        // Database context
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddSingleton<IDbExecutor>(sp => new DapperDbExecutor(connectionString));

        // HttpClients
        services.AddHttpClient<IPinquarkApiClient, PinquarkApiClient>()
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

        // Repositories
        services.AddScoped<IZoneRepository, ZoneRepository>();
        services.AddScoped<IZoneLocationRepository, ZoneLocationRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IWarehouseStockRepository, WarehouseStockRepository>();
        services.AddScoped<IDocRepository, DocRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ILogisticUnitRepository, LogisticUnitRepository>();
        services.AddScoped<IOperationRepository, OperationRepository>();
        services.AddScoped<ICustomOperationRepository, CustomOperationRepository>();

        // Services
        services.AddScoped<ISyncService, SyncService>();

        // Background worker
        services.AddHostedService<Worker>();

        // Host options
        services.Configure<HostOptions>(options => options.ShutdownTimeout = TimeSpan.FromSeconds(15));
    })
    .Build();

host.Run();