using PinquarkWMSToERPSynchro.Contracts.DTOs.Responses;
using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Infrastructure.Mappings
{
    public static class DocumentMapper
    {
        public static List<Document> MapToDocument(List<GetDocsResponse> responses)
        {
            var result = new List<Document>();

            foreach (var response in responses)
            {
                if (string.IsNullOrWhiteSpace(response.ErpDocId))
                {
                    continue;
                }

                var parts = response.ErpDocId.Split('|');
                if (parts.Length < 2)
                {
                    continue;
                }

                var erpIdPart = parts.Length == 2 ? parts[0] : parts[1];
                var erpTypePart = parts.Length == 2 ? parts[1] : parts[2];

                if (!int.TryParse(erpIdPart, out var erpId) || !int.TryParse(erpTypePart, out var erpType))
                {
                    continue;
                }

                result.Add(new Document
                {
                    Id = response.DocId,
                    Symbol = response.DocSymbol,
                    ErpId = erpId,
                    ErpType = erpType,
                    Status = response.DocStatus,
                    StatusId = response.DocStatusId,
                    Type = response.DocType,
                    TypeId = response.DocTypeId,
                    Quantity = response.QuantityOrdered,
                    QuantityRealized = response.QuantityDone
                });
            }

            return result;
        }
    }
}
