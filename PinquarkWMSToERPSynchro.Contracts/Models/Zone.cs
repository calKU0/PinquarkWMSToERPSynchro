namespace PinquarkWMSToERPSynchro.Contracts.Models
{
    public class Zone
    {
        public int Id { get; set; }
        public string ZoneName { get; set; }
        public string WarehouseName { get; set; }
        public string? ZoneType { get; set; }
    }
}
