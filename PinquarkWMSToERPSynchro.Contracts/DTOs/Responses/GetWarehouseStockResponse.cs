namespace PinquarkWMSToERPSynchro.Contracts.DTOs.Responses
{
    public class GetWarehouseStockResponse
    {
        public string ErpItemId { get; set; }
        public string ItemSymbol { get; set; }
        public string? LocationKanbanCode { get; set; }
        public int? LocationId { get; set; }
        public decimal Quantity { get; set; }
        public decimal OutOfStock { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime? TermValidity { get; set; }
        public List<Block> Blocks { get; set; }
    }

    public class Block
    {
        public string Type { get; set; }
        public decimal Quantity { get; set; }
    }
}
