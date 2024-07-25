using Quartz;

namespace QuartzJobManagementDemo.Chronos.QuartzJobs
{
    public class NotifierJob : IJob
    {
        public static readonly JobKey Key = new("notifier-job");

        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
