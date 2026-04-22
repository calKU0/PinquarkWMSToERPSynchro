using PinquarkWMSToERPSynchro.Contracts.DTOs;
using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Infrastructure.Mappings
{
    public static class LocationMapper
    {
        public static List<Location> MapToLocation(List<GetLocationsResponse> responses)
        {
            return responses
                .Select(r => new Location
                {
                    Id = r.LocationId,
                    LocationCode = r.LocationCode,
                    ZoneIds = r.ZoneIds ?? new List<int>(),
                    LocationEan = r.LocationEan,
                    LocationKanbanCode = r.LocationKanbanCode,
                    Warehouse = r.Warehouse,
                    WeightLoad = r.WeightLoad,
                    CapacityLoad = r.CapacityLoad,
                    Weight = r.LoadCapacity ?? r.MaxWeightLoad,
                    Volume = r.AttrVolume ?? r.MaxCapacityLoad,
                    Length = r.AttrLength ?? r.Length,
                    Width = r.AttrWidth ?? r.Width,
                    Height = r.AttrHeight ?? r.Height,
                    Active = r.Status,
                    LocationType = r.LocationType,
                    ProcessType = r.LocationProcessName,
                    MaxIndexQuantity = r.MaxIndexQuantity,
                    MaxBatchQuantity = r.MaxBatchQuantity,
                    RotationClass = r.RotationClass,
                    MaxJlQuantity = r.MaxJlQuantity,
                    MaxItemCount = r.MaxItemCount
                })
                .ToList();
        }

        public static List<LocationZone> MapToLocationZones(List<GetLocationsResponse> responses)
        {
            var result = new List<LocationZone>();

            foreach (var r in responses)
            {
                if (r.ZoneIds == null || !r.ZoneIds.Any())
                    continue;

                foreach (var zoneId in r.ZoneIds)
                {
                    result.Add(new LocationZone
                    {
                        ZoneId = zoneId,
                        LocationId = r.LocationId
                    });
                }
            }

            return result;
        }
    }
}
