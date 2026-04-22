namespace PinquarkWMSToERPSynchro.Contracts.Models
{
    public class CustomOperation
    {
        public int Id { get; set; }
        public int WmsId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int OperationId { get; set; }
        public string OperationName { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int DurationSeconds { get; set; }
    }
}
