namespace PinquarkWMSToERPSynchro.Contracts.DTOs.Responses
{
    public class GetDocsResponse
    {
        public int DocId { get; set; }
        public string DocSymbol { get; set; }
        public string? ErpDocId { get; set; }
        public string DocStatus { get; set; }
        public int DocStatusId { get; set; }
        public int DocTypeId { get; set; }
        public string DocType { get; set; }
        public decimal QuantityOrdered { get; set; }
        public decimal QuantityDone { get; set; }
    }
}
