namespace PinquarkWMSToERPSynchro.Contracts.Settings
{
    public class PinquarkApiSettings
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string WarehouseId { get; set; }
        public List<SyncEndpoint> Endpoints { get; set; } = new();
    }

    public class SyncEndpoint
    {
        public string Name { get; set; }
        public string Endpoint { get; set; }
        public int SyncIntervalMinutes { get; set; }
        public List<string> DependsOn { get; set; } = new();
    }
}
