using PinquarkWMSToERPSynchro.Contracts.DTOs.Responses;
using PinquarkWMSToERPSynchro.Contracts.Models;

namespace PinquarkWMSToERPSynchro.Infrastructure.Mappings
{
    public static class CustomOperationMapper
    {
        public static List<CustomOperation> MapToCustomOperation(List<GetCustomOperationResponse> responses)
        {
            return responses.Select(r => new CustomOperation
            {
                WmsId = r.Id,
                UserId = r.UserId,
                UserName = r.UserName,
                OperationId = r.OperationId,
                OperationName = r.OperationName,
                TimeStart = r.TimeStart,
                TimeEnd = r.TimeEnd,
                WarehouseId = r.WarehouseId,
                WarehouseName = r.WarehouseName,
                DurationSeconds = ParseDurationToSeconds(r.Duration)
            }).ToList();
        }

        private static int ParseDurationToSeconds(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
                return 0;

            try
            {
                // Parse duration in format like "07 d 06:24:00" (days + HH:MM:SS)
                var parts = duration.Split(new[] { " d " }, StringSplitOptions.None);

                int totalSeconds = 0;

                if (parts.Length == 2)
                {
                    // Has days component
                    if (int.TryParse(parts[0].Trim(), out var days))
                    {
                        totalSeconds += days * 24 * 60 * 60;
                    }

                    // Parse time component
                    if (TimeSpan.TryParse(parts[1].Trim(), out var timeSpan))
                    {
                        totalSeconds += (int)timeSpan.TotalSeconds;
                    }
                }
                else
                {
                    // No days component, just parse as TimeSpan
                    if (TimeSpan.TryParse(duration, out var timeSpan))
                    {
                        totalSeconds = (int)timeSpan.TotalSeconds;
                    }
                }

                return totalSeconds;
            }
            catch
            {
                return 0;
            }
        }
    }
}
