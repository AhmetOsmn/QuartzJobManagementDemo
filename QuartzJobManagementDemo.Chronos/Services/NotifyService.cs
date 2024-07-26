using Serilog;

namespace QuartzJobManagementDemo.Chronos.Services
{
    public static class NotifyService
    {
        public static void Notify(string to, string message)
        {
            Log.Information($"Notify sent to {to} with message {message}");
        }
    }
}
