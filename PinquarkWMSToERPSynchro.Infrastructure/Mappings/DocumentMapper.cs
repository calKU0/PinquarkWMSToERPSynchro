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
                int? erpId = null;
                int? erpType = null;

                if (!string.IsNullOrWhiteSpace(response.ErpDocId))
                {
                    var parts = response.ErpDocId.Split('|');

                    if (parts.Length >= 2)
                    {
                        var erpIdPart = parts.Length == 2 ? parts[0] : parts[1];
                        var erpTypePart = parts.Length == 2 ? parts[1] : parts[2];

                        if (int.TryParse(erpIdPart, out var parsedId))
                            erpId = parsedId;

                        if (int.TryParse(erpTypePart, out var parsedType))
                            erpType = parsedType;
                    }
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
