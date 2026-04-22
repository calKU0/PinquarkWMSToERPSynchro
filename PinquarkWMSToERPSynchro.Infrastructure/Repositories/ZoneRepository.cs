using Dapper;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using System.Data;

namespace PinquarkWMSToERPSynchro.Infrastructure.Repositories
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public ZoneRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task UpsertAsync(IEnumerable<Zone> items, CancellationToken cancellationToken = default)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
                return;

            const string storedProcedure = "wms.UpsertZonesBulk";

            var dataTable = new DataTable();
            dataTable.Columns.Add("Str_ID", typeof(int));
            dataTable.Columns.Add("Str_Nazwa", typeof(string));
            dataTable.Columns.Add("Str_Typ", typeof(string));
            dataTable.Columns.Add("Str_Magazyn", typeof(string));

            foreach (var item in itemsList)
            {
                dataTable.Rows.Add(
                    item.Id,
                    item.ZoneName,
                    item.ZoneType,
                    item.WarehouseName
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", dataTable.AsTableValuedParameter("wms.ZonesTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }
    }
}
