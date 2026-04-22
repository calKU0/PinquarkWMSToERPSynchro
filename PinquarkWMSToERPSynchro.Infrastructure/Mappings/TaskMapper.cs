using PinquarkWMSToERPSynchro.Contracts.DTOs.Responses;
using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Infrastructure.Mappings
{
    public static class TaskMapper
    {
        public static List<TaskModel> MapToTask(List<GetTasksResponse> responses)
        {
            return responses.Select(r => new TaskModel
            {
                Id = r.TaskId,
                Symbol = r.Symbol,
                Date = r.Date,
                DateReal = r.DateReal,
                PickingType = r.PickingType,
                PickingTypeId = r.PickingType.PickingTypeId(),
                SourceZoneId = r.SourceZoneId,
                TargetZoneId = r.TargetZoneId,
                TaskStatus = r.TaskStatus,
                TaskStatusId = r.TaskStatusId,
                OrderDocIds = r.OrderDocIds ?? new List<int>()
            }).ToList();
        }

        private static int PickingTypeId(this string pickingType)
        {
            return pickingType switch
            {
                "Picking" => 1,
                "Multipicking" => 2,
                "Multipicking One" => 3,
                _ => 4
            };
        }
    }
}
