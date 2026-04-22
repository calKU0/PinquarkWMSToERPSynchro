namespace PinquarkWMSToERPSynchro.Contracts.DTOs.Responses
{
    public class GetTasksResponse
    {
        public int TaskId { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateReal { get; set; }
        public string PickingType { get; set; }
        public string SourceZone { get; set; }
        public int SourceZoneId { get; set; }
        public string TargetZone { get; set; }
        public int TargetZoneId { get; set; }
        public int TaskStatusId { get; set; }
        public string TaskStatus { get; set; }
        public List<int> OrderDocIds { get; set; }
    }
}
