using Dapper;
using PinquarkWMSToERPSynchro.Contracts.Models;
using PinquarkWMSToERPSynchro.Contracts.Repositories;
using PinquarkWMSToERPSynchro.Infrastructure.Data;
using System.Data;


namespace PinquarkWMSToERPSynchro.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbExecutor _dbExecutor;

        public TaskRepository(IDbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task UpsertAsync(IEnumerable<TaskModel> items, CancellationToken cancellationToken = default)
        {
            var itemsList = items.ToList();
            if (!itemsList.Any())
            {
                return;
            }

            await UpsertTasksAsync(itemsList);
            await UpsertTaskDocumentsAsync(itemsList);
        }

        private async Task UpsertTasksAsync(List<TaskModel> items)
        {
            const string storedProcedure = "wms.UpsertTasksBulk";

            var dataTable = new DataTable();
            dataTable.Columns.Add("Zad_Id", typeof(int));
            dataTable.Columns.Add("Zad_Kod", typeof(string));
            dataTable.Columns.Add("Zad_PickingTyp", typeof(string));
            dataTable.Columns.Add("Zad_PickingTypId", typeof(int));
            dataTable.Columns.Add("Zad_StrefaZrdId", typeof(int));
            dataTable.Columns.Add("Zad_StrefaDocId", typeof(int));
            dataTable.Columns.Add("Zad_Status", typeof(string));
            dataTable.Columns.Add("Zad_StatusId", typeof(int));
            dataTable.Columns.Add("Zad_Data", typeof(DateTime));
            dataTable.Columns.Add("Zad_DataRealizacji", typeof(DateTime));

            foreach (var item in items)
            {
                dataTable.Rows.Add(
                    item.Id,
                    item.Symbol,
                    item.PickingType,
                    item.PickingTypeId,
                    item.SourceZoneId,
                    item.TargetZoneId,
                    item.TaskStatus,
                    item.TaskStatusId,
                    item.Date,
                    item.DateReal
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", dataTable.AsTableValuedParameter("wms.TasksTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }

        private async Task UpsertTaskDocumentsAsync(List<TaskModel> items)
        {
            const string storedProcedure = "wms.UpsertTaskDocumentsBulk";

            var relations = items
                .Where(x => x.OrderDocIds != null)
                .SelectMany(x => x.OrderDocIds.Select(docId => new { TaskId = x.Id, DocumentId = docId }))
                .Distinct()
                .ToList();

            if (!relations.Any())
            {
                return;
            }

            var dataTable = new DataTable();
            dataTable.Columns.Add("ZD_ZadanieId", typeof(int));
            dataTable.Columns.Add("ZD_DokumentId", typeof(int));

            foreach (var relation in relations)
            {
                dataTable.Rows.Add(relation.TaskId, relation.DocumentId);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Items", dataTable.AsTableValuedParameter("wms.TaskDocumentsTableType"));

            await _dbExecutor.ExecuteAsync(storedProcedure, parameters, CommandType.StoredProcedure);
        }
    }
}
