using PinquarkWMSToERPSynchro.Contracts.DTOs.Responses;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Infrastructure.Helpers;

namespace PinquarkWMSToERPSynchro.Infrastructure.Mappings
{
    public static class LogisticUnitMapper
    {
        public static List<LogisticUnit> MapToLogisticUnits(List<GetLuResourcesResponse> responses)
        {
            return responses.Select(r => new LogisticUnit
            {
                Id = r.LuId,
                Code = Utils.SafeSubstring(r.LuCode, 100)!,
                Status = r.LuStatus,
                ZoneId = r.ZoneId is null ? null : int.Parse(r.ZoneId.Split(",").First()),
                LocationId = r.LocationId,
                TargetZoneId = r.TargetZoneId,
                PutawayType = r.PutawayType,
                Content = r.Content?
                    .GroupBy(c => new
                    {
                        c.ItemErpId,
                        c.BatchNumber,
                        c.TermValidity
                    })
                    .Select(g => new LogisticUnitContent
                    {
                        ItemErpId = Convert.ToInt32(g.Key.ItemErpId),
                        BatchNumber = g.Key.BatchNumber,
                        TermValidity = g.Key.TermValidity,
                        Quantity = g.Sum(x => x.Quantity),
                        OutOfStock = g.Sum(x => x.OutOfStock)
                    })
                    .ToList() ?? new List<LogisticUnitContent>()
            }).ToList();
        }
    }
}
