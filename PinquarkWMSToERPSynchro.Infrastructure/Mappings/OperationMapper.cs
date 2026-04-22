using PinquarkWMSToERPSynchro.Contracts.DTOs.Responses;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Infrastructure.Helpers;

namespace PinquarkWMSToERPSynchro.Infrastructure.Mappings
{
    public static class OperationMapper
    {
        public static List<Operations> MapToOperations(List<GetOperationsResponse> responses)
        {
            var result = new List<Operations>();

            foreach (var response in responses)
            {
                ParseErpMainDocument(response.MainDocErpId, out var erpDocId, out var erpDocTyp);

                result.Add(new Operations
                {
                    Id = response.Id,
                    Symbol = response.MainDocSymbol ?? response.DocSymbol,
                    ErpDocId = erpDocId,
                    ErpDocTyp = erpDocTyp,
                    ErpItemId = int.Parse(response.ItemErpId),
                    Quantity = response.Quantity,
                    LuFromId = response.LuFromId,
                    LocationFromId = response.LocationFromId,
                    LuToId = response.LuToId,
                    LocationToId = response.LocationToId,
                    User = response.User,
                    OperationType = response.OperationType,
                    OperationTypeId = response.OperationTypeId,
                    DocStatus = response.DocStatus,
                    DocStatusId = response.DocStatusId,
                    DocDate = response.DocDate,
                    OperationDate = response.OperationDate,
                    OperationDateEnd = response.OperationDateEnd,
                    BatchNumber = response.BatchNumber,
                    TermValidity = response.TermValidity,
                    LuTypeTo = Utils.SafeSubstring(response.LuTypeTo, 100),
                    LuTypeFrom = Utils.SafeSubstring(response.LuTypeFrom, 100),
                    ZoneFromId = response.ZoneFromId?.Any() == true
                    ? response.ZoneFromId.First()
                    : null,
                    ZoneToId = response.ZoneToId?.Any() == true
                    ? response.ZoneToId.First()
                    : null
                });
            }

            return result;
        }

        private static void ParseErpMainDocument(string? rawValue, out int? erpId, out int? erpType)
        {
            erpId = null;
            erpType = null;

            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return;
            }

            var parts = rawValue.Split('|');
            if (parts.Length < 2)
            {
                return;
            }

            var erpIdPart = parts.Length == 2 ? parts[0] : parts[1];
            var erpTypePart = parts.Length == 2 ? parts[1] : parts[2];

            if (int.TryParse(erpIdPart, out var parsedId))
            {
                erpId = parsedId;
            }

            if (int.TryParse(erpTypePart, out var parsedType))
            {
                erpType = parsedType;
            }
        }
    }
}
