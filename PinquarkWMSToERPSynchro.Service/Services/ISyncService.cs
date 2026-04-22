namespace PinquarkWMSToERPSynchro.Service.Services
{
    public interface ISyncService
    {
        Task SyncAsync(string endpointName, string endpointUrl, CancellationToken cancellationToken = default);
    }
}
