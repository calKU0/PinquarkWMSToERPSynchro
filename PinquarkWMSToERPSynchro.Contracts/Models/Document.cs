namespace PinquarkWMSToERPSynchro.Contracts.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public int? ErpId { get; set; }
        public int? ErpType { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string Type { get; set; }
        public int TypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityRealized { get; set; }
    }
}
