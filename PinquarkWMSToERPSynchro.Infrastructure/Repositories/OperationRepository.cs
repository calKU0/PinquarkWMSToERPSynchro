using Dapper;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using System.Data;

namespace PinquarkWMSToERPSynchro.Infrastructure.Repositories
{
    public class OperationRepository : IOperationRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public OperationRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<DateTime> GetLastSyncDateAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT MAX(Ope_Data) FROM wms.Operacje";
            var result = await _dbExecutor.QuerySingleOrDefaultAsync<DateTime?>(sql);

            return result ?? new DateTime(1900, 1, 1);
        }

        public async Task UpsertAsync(IEnumerable<Operations> items, CancellationToken cancellationToken = default)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
            {
                return;
            }

            const string storedProcedure = "wms.UpsertOperationsBulk";

            var dataTable = new DataTable();
            dataTable.Columns.Add("Ope_Id", typeof(int));
            dataTable.Columns.Add("Ope_Kod", typeof(string));
            dataTable.Columns.Add("Ope_ErpDokId", typeof(int));
            dataTable.Columns.Add("Ope_ErpDokTyp", typeof(int));
            dataTable.Columns.Add("Ope_ErpTwrId", typeof(int));
            dataTable.Columns.Add("Ope_Ilosc", typeof(decimal));
            dataTable.Columns.Add("Ope_JlZrdId", typeof(int));
            dataTable.Columns.Add("Ope_LokZrdId", typeof(int));
            dataTable.Columns.Add("Ope_JlDocId", typeof(int));
            dataTable.Columns.Add("Ope_LokDocId", typeof(int));
            dataTable.Columns.Add("Ope_Operator", typeof(string));
            dataTable.Columns.Add("Ope_OperacjaTyp", typeof(string));
            dataTable.Columns.Add("Ope_OperacjaTypId", typeof(int));
            dataTable.Columns.Add("Ope_DokStatus", typeof(string));
            dataTable.Columns.Add("Ope_DokStatusId", typeof(int));
            dataTable.Columns.Add("Ope_DokData", typeof(DateTime));
            dataTable.Columns.Add("Ope_Data", typeof(DateTime));
            dataTable.Columns.Add("Ope_DataKoniec", typeof(DateTime));
            dataTable.Columns.Add("Ope_NrParti", typeof(string));
            dataTable.Columns.Add("Ope_DataWaznosci", typeof(DateTime));
            dataTable.Columns.Add("Ope_JlTypZrd", typeof(string));
            dataTable.Columns.Add("Ope_JlTypDoc", typeof(string));
            dataTable.Columns.Add("Ope_StrefaZrdId", typeof(int));
            dataTable.Columns.Add("Ope_StrefaDocId", typeof(int));

            foreach (var item in itemsList)
            {
                dataTable.Rows.Add(
                    item.Id,
                    item.Symbol,
                    item.ErpDocId ?? (object)DBNull.Value,
                    item.ErpDocTyp ?? (object)DBNull.Value,
                    item.ErpItemId,
                    item.Quantity,
                    item.LuFromId ?? (object)DBNull.Value,
                    item.LocationFromId ?? (object)DBNull.Value,
                    item.LuToId ?? (object)DBNull.Value,
                    item.LocationToId ?? (object)DBNull.Value,
                    item.User,
                    item.OperationType,
                    item.OperationTypeId,
                    item.DocStatus,
                    item.DocStatusId,
                    item.DocDate,
                    item.OperationDate,
                    item.OperationDateEnd ?? (object)DBNull.Value,
                    item.BatchNumber ?? (object)DBNull.Value,
                    item.TermValidity ?? (object)DBNull.Value,
                    item.LuTypeFrom ?? (object)DBNull.Value,
                    item.LuTypeTo ?? (object)DBNull.Value,
                    item.ZoneFromId ?? (object)DBNull.Value,
                    item.ZoneToId ?? (object)DBNull.Value
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", dataTable.AsTableValuedParameter("wms.OperationsTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }
    }
}
