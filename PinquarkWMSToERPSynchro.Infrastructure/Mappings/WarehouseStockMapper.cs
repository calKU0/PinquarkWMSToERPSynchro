using PinquarkWMSToERPSynchro.Contracts.DTOs.Responses;
using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Infrastructure.Mappings
{
    public static class WarehouseStockMapper
    {
        public static List<WarehouseStock> MapToWarehouseStock(List<GetWarehouseStockResponse> responses)
        {
            return responses
                .Where(r => r.LocationId != null)
                .GroupBy(r => new
                {
                    r.LocationId,
                    r.BatchNumber,
                    r.ErpItemId
                })
                .Select(g => new WarehouseStock
                {
                    ProductId = int.Parse(g.Key.ErpItemId),
                    LocationId = g.Key.LocationId!.Value,
                    BatchNumber = g.Key.BatchNumber,
                    Quantity = g.Sum(x => x.Quantity),
                    OutOfStock = g.Sum(x => x.OutOfStock),
                    TermValidity = g.FirstOrDefault()?.TermValidity
                })
                .ToList();
        }
    }
}
