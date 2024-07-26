using Quartz;
using Serilog;

namespace QuartzJobManagementDemo.Shared.QuartzJobs
{
    public class ThirPartyJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information("Third party job is started");
            await Task.Delay(1000);
            Log.Information("Third party job is completed");
        }
    }
}
