using System.Text.Json.Serialization;

namespace PinquarkWMSToERPSynchro.Contracts.DTOs.Requests
{
    public class GetTasksRequest
    {
        [JsonPropertyName("dateFrom")]
        public DateTime DateFrom { get; set; } = DateTime.MinValue;
        [JsonPropertyName("dateTo")]
        public DateTime DateTo { get; set; } = DateTime.Now;
        [JsonPropertyName("docStatusIds")]
        public List<int>? Statuses { get; set; }

    }
}
