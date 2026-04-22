using Dapper;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using System.Data;

namespace PinquarkWMSToERPSynchro.Infrastructure.Repositories
{
    public class WarehouseStockRepository : IWarehouseStockRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public WarehouseStockRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task UpsertAsync(IEnumerable<WarehouseStock> items, CancellationToken cancellationToken = default)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
                return;

            const string storedProcedure = "wms.UpsertWarehouseStockBulk";

            var dataTable = new DataTable();
            dataTable.Columns.Add("ProductId", typeof(int));
            dataTable.Columns.Add("LocationId", typeof(int));
            dataTable.Columns.Add("Quantity", typeof(decimal));
            dataTable.Columns.Add("OutOfStock", typeof(decimal));
            dataTable.Columns.Add("BatchNumber", typeof(string));
            dataTable.Columns.Add("TermValidity", typeof(DateTime));

            foreach (var item in itemsList)
            {
                dataTable.Rows.Add(
                    item.ProductId,
                    item.LocationId,
                    item.Quantity,
                    item.OutOfStock,
                    item.BatchNumber ?? (object)DBNull.Value,
                    item.TermValidity ?? (object)DBNull.Value
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", dataTable.AsTableValuedParameter("wms.WarehouseStockTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }
    }
}
