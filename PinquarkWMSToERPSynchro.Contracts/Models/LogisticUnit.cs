namespace PinquarkWMSToERPSynchro.Contracts.Models
{
    public class LogisticUnit
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public List<LogisticUnitContent> Content { get; set; } = new();
        public int? ZoneId { get; set; }
        public int? LocationId { get; set; }
        public int? TargetZoneId { get; set; }
        public string? PutawayType { get; set; }
    }
}
