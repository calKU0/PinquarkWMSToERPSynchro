namespace PinquarkWMSToERPSynchro.Contracts.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string LocationCode { get; set; }
        public List<int> ZoneIds { get; set; }

        public string LocationEan { get; set; }
        public string LocationKanbanCode { get; set; }
        public string Warehouse { get; set; }

        public decimal WeightLoad { get; set; }
        public decimal CapacityLoad { get; set; }

        public decimal Weight { get; set; }
        public decimal Volume { get; set; }

        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public int Active { get; set; }
        public string? ProcessType { get; set; }
        public string? LocationType { get; set; }

        public int? MaxIndexQuantity { get; set; }
        public int? MaxBatchQuantity { get; set; }

        public string RotationClass { get; set; }

        public int? MaxJlQuantity { get; set; }
        public int? MaxItemCount { get; set; }
    }
}
