namespace PinquarkWMSToERPSynchro.Contracts.Models
{
    public class Operations
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public int? ErpDocId { get; set; }
        public int? ErpDocTyp { get; set; }
        public int ErpItemId { get; set; }
        public decimal Quantity { get; set; }
        public int? LuFromId { get; set; }
        public int? LocationFromId { get; set; }
        public int? LuToId { get; set; }
        public int? LocationToId { get; set; }
        public string User { get; set; }
        public string OperationType { get; set; }
        public int OperationTypeId { get; set; }
        public string DocStatus { get; set; }
        public int DocStatusId { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime OperationDate { get; set; }
        public DateTime? OperationDateEnd { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime? TermValidity { get; set; }
        public string? LuTypeTo { get; set; }
        public string? LuTypeFrom { get; set; }
        public int? ZoneFromId { get; set; }
        public int? ZoneToId { get; set; }
    }
}
