namespace PinquarkWMSToERPSynchro.Contracts.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateReal { get; set; }
        public string PickingType { get; set; }
        public int PickingTypeId { get; set; }
        public int SourceZoneId { get; set; }
        public int TargetZoneId { get; set; }
        public string TaskStatus { get; set; }
        public int TaskStatusId { get; set; }
        public List<int> OrderDocIds { get; set; }
    }
}
