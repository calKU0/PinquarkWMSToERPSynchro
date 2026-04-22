namespace PinquarkWMSToERPSynchro.Contracts.DTOs
{
    public class GetLocationsResponse
    {
        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        public List<int> ZoneIds { get; set; }

        public string LocationEan { get; set; }
        public string LocationKanbanCode { get; set; }
        public string Warehouse { get; set; }

        public decimal WeightLoad { get; set; }
        public decimal WeightPercent { get; set; }

        public decimal CapacityLoad { get; set; }
        public decimal CapacityPercent { get; set; }

        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public decimal MaxCapacityLoad { get; set; }
        public decimal MaxWeightLoad { get; set; }

        public int Status { get; set; }
        public int LocationTypeId { get; set; }
        public string? LocationType { get; set; }
        public string? LocationProcessName { get; set; }

        public int? MaxIndexQuantity { get; set; }
        public int? MaxBatchQuantity { get; set; }

        public decimal? AttrWidth { get; set; }
        public decimal? AttrLength { get; set; }
        public decimal? AttrHeight { get; set; }
        public decimal? AttrVolume { get; set; }

        public int? LoadCapacity { get; set; }

        public string RotationClass { get; set; }

        //public string? LocationBlockReason { get; set; }
        //public string? Reference { get; set; }

        //public decimal? LocationVolume { get; set; }
        public int? MaxJlQuantity { get; set; }
        public int? MaxItemCount { get; set; }
    }
}
