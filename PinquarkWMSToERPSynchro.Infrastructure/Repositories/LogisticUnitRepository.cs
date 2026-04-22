using Dapper;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using System.Data;

namespace PinquarkWMSToERPSynchro.Infrastructure.Repositories
{
    public class LogisticUnitRepository : ILogisticUnitRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public LogisticUnitRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task UpsertAsync(IEnumerable<LogisticUnit> items, CancellationToken cancellationToken = default)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
            {
                return;
            }

            const string storedProcedure = "wms.UpsertLogisticUnitsBulk";

            var logisticUnitsTable = new DataTable();
            logisticUnitsTable.Columns.Add("Jl_Id", typeof(int));
            logisticUnitsTable.Columns.Add("Jl_Kod", typeof(string));
            logisticUnitsTable.Columns.Add("Jl_Status", typeof(string));
            logisticUnitsTable.Columns.Add("Jl_StrefaId", typeof(int));
            logisticUnitsTable.Columns.Add("Jl_LokId", typeof(string));
            logisticUnitsTable.Columns.Add("Jl_StrefaDocId", typeof(int));
            logisticUnitsTable.Columns.Add("Jl_TypOdkladania", typeof(string));

            foreach (var item in itemsList)
            {
                logisticUnitsTable.Rows.Add(
                    item.Id,
                    item.Code,
                    item.Status,
                    item.ZoneId ?? (object)DBNull.Value,
                    item.LocationId ?? (object)DBNull.Value,
                    item.TargetZoneId ?? (object)DBNull.Value,
                    item.PutawayType ?? (object)DBNull.Value
                );
            }

            var contentTable = new DataTable();
            contentTable.Columns.Add("JLS_JlId", typeof(int));
            contentTable.Columns.Add("JLS_TwrNumer", typeof(int));
            contentTable.Columns.Add("JLS_NrParti", typeof(string));
            contentTable.Columns.Add("JLS_DataWaznosci", typeof(string));
            contentTable.Columns.Add("JLS_Ilosc", typeof(decimal));
            contentTable.Columns.Add("JLS_Braki", typeof(decimal));

            foreach (var item in itemsList)
            {
                foreach (var content in item.Content)
                {
                    contentTable.Rows.Add(
                        item.Id,
                        content.ItemErpId,
                        content.BatchNumber ?? (object)DBNull.Value,
                        content.TermValidity ?? (object)DBNull.Value,
                        content.Quantity,
                        content.OutOfStock
                    );
                }
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", logisticUnitsTable.AsTableValuedParameter("wms.LogisticUnitsTableType"));
            parameters.Add("@ContentItems", contentTable.AsTableValuedParameter("wms.LogisticUnitContentTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }
    }
}
