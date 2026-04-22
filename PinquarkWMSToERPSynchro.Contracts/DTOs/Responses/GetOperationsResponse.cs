namespace PinquarkWMSToERPSynchro.Contracts.DTOs.Responses
{
    public class GetOperationsResponse
    {
        public decimal Quantity { get; set; }
        public string DocSymbol { get; set; }
        public string ItemErpId { get; set; }
        public string ItemName { get; set; }
        public string ItemSymbol { get; set; }
        public string? LuFrom { get; set; }
        public int? LuFromId { get; set; }
        public string? LocationFrom { get; set; }
        public int? LocationFromId { get; set; }
        public int Id { get; set; }
        public string? LuTo { get; set; }
        public int? LuToId { get; set; }
        public string? LocationTo { get; set; }
        public int? LocationToId { get; set; }
        public string User { get; set; }
        public int UserId { get; set; }
        public int OperationTypeId { get; set; }
        public string OperationType { get; set; }
        public int DocStatusId { get; set; }
        public string DocStatus { get; set; }
        public DateTime DocDate { get; set; }
        public int Lp { get; set; }
        public DateTime OperationDate { get; set; }
        public DateTime? OperationDateEnd { get; set; }
        public int WarehouseId { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime? TermValidity { get; set; }
        public string? MainDocSymbol { get; set; }
        public string? MainDocErpId { get; set; }
        public string? LuTypeTo { get; set; }
        public string? LuTypeFrom { get; set; }
        public string? ZoneFrom { get; set; }
        public List<int>? ZoneFromId { get; set; }
        public string? ZoneTo { get; set; }
        public List<int>? ZoneToId { get; set; }
    }
}
