namespace PinquarkWMSToERPSynchro.Infrastructure.Helpers
{
    public static class Utils
    {
        public static string? SafeSubstring(string? value, int maxLength)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : (value.Length <= maxLength ? value : value.Substring(0, maxLength));
        }
    }
}
