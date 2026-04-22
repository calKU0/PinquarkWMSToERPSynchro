using Dapper;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using System.Data;

namespace PinquarkWMSToERPSynchro.Infrastructure.Repositories
{
    public class ZoneLocationRepository : IZoneLocationRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public ZoneLocationRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task UpsertAsync(IEnumerable<LocationZone> items, CancellationToken cancellationToken = default)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
                return;

            const string storedProcedure = "wms.UpsertZoneLocationsBulk";

            var dataTable = new DataTable();
            dataTable.Columns.Add("SL_StrefaId", typeof(int));
            dataTable.Columns.Add("SL_LokalizacjaId", typeof(int));

            foreach (var item in itemsList)
            {
                dataTable.Rows.Add(
                    item.ZoneId,
                    item.LocationId
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", dataTable.AsTableValuedParameter("wms.ZoneLocationsTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }
    }
}
