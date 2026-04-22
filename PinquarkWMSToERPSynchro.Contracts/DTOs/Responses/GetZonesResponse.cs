namespace PinquarkWMSToERPSynchro.Contracts.DTOs.Responses
{
    public class GetZonesResponse
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string WarehouseName { get; set; }
        public string? ZoneType { get; set; }
    }
}
