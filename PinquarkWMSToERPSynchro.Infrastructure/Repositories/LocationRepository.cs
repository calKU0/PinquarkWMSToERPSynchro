using Dapper;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using System.Data;

namespace PinquarkWMSToERPSynchro.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public LocationRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task UpsertAsync(IEnumerable<Location> items, CancellationToken cancellationToken = default)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
                return;

            const string storedProcedure = "wms.UpsertLocationsBulk";

            var dataTable = new DataTable();
            dataTable.Columns.Add("Lok_Id", typeof(int));
            dataTable.Columns.Add("Lok_KodWMS", typeof(string));
            dataTable.Columns.Add("Lok_KodERP", typeof(string));
            dataTable.Columns.Add("Lok_Ean", typeof(string));
            dataTable.Columns.Add("Lok_Magazyn", typeof(string));
            dataTable.Columns.Add("Lok_Waga", typeof(int));
            dataTable.Columns.Add("Lok_Objetosc", typeof(decimal));
            dataTable.Columns.Add("Lok_Dlugosc", typeof(decimal));
            dataTable.Columns.Add("Lok_Szerokosc", typeof(decimal));
            dataTable.Columns.Add("Lok_Wysokosc", typeof(decimal));
            dataTable.Columns.Add("Lok_ZajetosWagi", typeof(int));
            dataTable.Columns.Add("Lok_ZajetoscObjetosci", typeof(int));
            dataTable.Columns.Add("Lok_Aktywna", typeof(bool));
            dataTable.Columns.Add("Lok_Typ", typeof(string));
            dataTable.Columns.Add("Lok_TypProcesu", typeof(string));
            dataTable.Columns.Add("Lok_MaxSKU", typeof(int));
            dataTable.Columns.Add("Lok_MaxPartie", typeof(int));
            dataTable.Columns.Add("Lok_MaxJl", typeof(int));
            dataTable.Columns.Add("Lok_MaxIlosc", typeof(int));
            dataTable.Columns.Add("Lok_KlasaRotacji", typeof(string));

            foreach (var item in itemsList)
            {
                dataTable.Rows.Add(
                    item.Id,
                    item.LocationCode,
                    item.LocationKanbanCode ?? (object)DBNull.Value,
                    item.LocationEan ?? (object)DBNull.Value,
                    item.Warehouse,
                    item.Weight,
                    item.Volume,
                    item.Length,
                    item.Width,
                    item.Height,
                    item.WeightLoad,
                    item.CapacityLoad,
                    item.Active == 1,
                    item.LocationType,
                    item.ProcessType,
                    item.MaxIndexQuantity,
                    item.MaxBatchQuantity ?? (object)DBNull.Value,
                    item.MaxJlQuantity ?? (object)DBNull.Value,
                    item.MaxItemCount ?? (object)DBNull.Value,
                    item.RotationClass ?? (object)DBNull.Value
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", dataTable.AsTableValuedParameter("wms.LocationsTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }
    }
}
