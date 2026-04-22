using Dapper;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using System.Data;

namespace PinquarkWMSToERPSynchro.Infrastructure.Repositories
{
    public class DocRepository : IDocRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public DocRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task UpsertAsync(IEnumerable<Document> items, CancellationToken cancellationToken = default)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
            {
                return;
            }

            const string storedProcedure = "wms.UpsertDocumentsBulk";

            var dataTable = new DataTable();
            dataTable.Columns.Add("Dok_Id", typeof(int));
            dataTable.Columns.Add("Dok_Kod", typeof(string));
            dataTable.Columns.Add("Dok_ErpId", typeof(int));
            dataTable.Columns.Add("Dok_ErpTyp", typeof(int));
            dataTable.Columns.Add("Dok_Status", typeof(string));
            dataTable.Columns.Add("Dok_StatusId", typeof(int));
            dataTable.Columns.Add("Dok_Typ", typeof(string));
            dataTable.Columns.Add("Dok_TypId", typeof(int));
            dataTable.Columns.Add("Dok_Ilosc", typeof(decimal));
            dataTable.Columns.Add("Dok_IloscZrealizowana", typeof(decimal));

            foreach (var item in itemsList)
            {
                dataTable.Rows.Add(
                    item.Id,
                    item.Symbol,
                    item.ErpId,
                    item.ErpType,
                    item.Status,
                    item.StatusId,
                    item.Type,
                    item.TypeId,
                    item.Quantity,
                    item.QuantityRealized
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", dataTable.AsTableValuedParameter("wms.DocumentsTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }
    }
}
