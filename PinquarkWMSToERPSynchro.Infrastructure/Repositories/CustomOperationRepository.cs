using Dapper;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using System.Data;

namespace PinquarkWMSToERPSynchro.Infrastructure.Repositories
{
    public class CustomOperationRepository : ICustomOperationRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public CustomOperationRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<DateTime> GetLastSyncDateAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT MAX(OPZ_DataZakonczenia) FROM wms.OperacjePozaTerminalem";

            var result = await _dbExecutor.QuerySingleOrDefaultAsync<DateTime?>(sql);

            return result ?? new DateTime(1900, 1, 1);
        }

        public async Task UpsertAsync(IEnumerable<CustomOperation> items, CancellationToken cancellationToken = default)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
                return;

            const string storedProcedure = "wms.UpsertCustomOperationsBulk";

            var dataTable = new DataTable();
            dataTable.Columns.Add("WmsId", typeof(int));
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("OperationId", typeof(int));
            dataTable.Columns.Add("OperationName", typeof(string));
            dataTable.Columns.Add("TimeStart", typeof(DateTime));
            dataTable.Columns.Add("TimeEnd", typeof(DateTime));
            dataTable.Columns.Add("WarehouseId", typeof(int));
            dataTable.Columns.Add("WarehouseName", typeof(string));
            dataTable.Columns.Add("DurationSeconds", typeof(int));

            foreach (var item in itemsList)
            {
                dataTable.Rows.Add(
                    item.WmsId,
                    item.UserId,
                    item.UserName,
                    item.OperationId,
                    item.OperationName,
                    item.TimeStart,
                    item.TimeEnd,
                    item.WarehouseId,
                    item.WarehouseName,
                    item.DurationSeconds
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", dataTable.AsTableValuedParameter("wms.OperacjePozaTerminalemTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }
    }
}
