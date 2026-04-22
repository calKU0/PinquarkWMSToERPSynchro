namespace PinquarkWMSToERPSynchro.Contracts.DTOs.Responses
{
    public class GetLuResourcesResponse
    {
        public int LuId { get; set; }
        public string LuCode { get; set; }
        public int LuStatusId { get; set; }
        public string LuStatus { get; set; }
        public List<ContentItem>? Content { get; set; }
        public string? Zone { get; set; }
        public string? ZoneId { get; set; }
        public string? Location { get; set; }
        public int? LocationId { get; set; }
        public string? TargetZone { get; set; }
        public int? TargetZoneId { get; set; }
        public string? PutawayType { get; set; }

        public class ContentItem
        {
            public string ItemErpId { get; set; }
            public string ItemSymbol { get; set; }
            public string? BatchNumber { get; set; }
            public string? TermValidity { get; set; }
            public decimal Quantity { get; set; }
            public decimal OutOfStock { get; set; }
        }
    }
}
