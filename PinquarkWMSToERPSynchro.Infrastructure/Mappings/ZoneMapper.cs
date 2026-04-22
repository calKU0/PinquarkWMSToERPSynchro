using PinquarkWMSToERPSynchro.Contracts.DTOs.Responses;
using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Infrastructure.Mappings
{
    public static class ZoneMapper
    {
        public static List<Zone> MapToZone(List<GetZonesResponse> responses)
        {
            return responses.Select(r => new Zone
            {
                Id = r.ZoneId,
                ZoneName = r.ZoneName,
                WarehouseName = r.WarehouseName,
                ZoneType = r.ZoneType
            }).ToList();
        }
    }
}
