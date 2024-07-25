
using Serilog;

namespace QuartzJobManagementDemo.Chronos
{
    public static class SmtpService
    {
        public static void SendEmail(string to, string subject, string body)
        {
            // Send email
            Log.Information($"Email sent to {to} with subject {subject} and body {body}");
        }
    }
}
