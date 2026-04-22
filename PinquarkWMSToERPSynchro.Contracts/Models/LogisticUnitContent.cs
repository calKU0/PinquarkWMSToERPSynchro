namespace PinquarkWMSToERPSynchro.Contracts.Models
{
    public class LogisticUnitContent
    {
        public int ItemErpId { get; set; }
        public string? BatchNumber { get; set; }
        public string? TermValidity { get; set; }
        public decimal Quantity { get; set; }
        public decimal OutOfStock { get; set; }
    }
}
