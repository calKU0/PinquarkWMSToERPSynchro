using System.Text.Json.Serialization;

namespace PinquarkWMSToERPSynchro.Contracts.DTOs.Requests
{
    public class GetLocationsRequest
    {
        [JsonPropertyName("warehouse")]
        public string? Warehouse { get; set; }
    }
}
