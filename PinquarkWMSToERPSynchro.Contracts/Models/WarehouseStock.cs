namespace PinquarkWMSToERPSynchro.Contracts.Models
{
    public class WarehouseStock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int LocationId { get; set; }
        public decimal Quantity { get; set; }
        public decimal OutOfStock { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime? TermValidity { get; set; }
    }
}
