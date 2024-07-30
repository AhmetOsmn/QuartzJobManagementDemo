using Quartz;
using Quartz.Listener;
using Serilog;

namespace QuartzJobManagementDemo.Chronos.QuartzJobs.JobListeners
{
    public class MessagePrinterJobListener : JobListenerSupport
    {
        public override string Name => "MessagePrinterJobListener";

        public override Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Log.Information($"#####Job: {context.JobDetail.Key.Name} execution vetoed");
            return Task.CompletedTask;
        }

        public override Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Log.Information($"#####Job: {context.JobDetail.Key.Name} is about to be executed");
            return Task.CompletedTask;
        }

        public override Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
        {
            Log.Information($"#####Job: {context.JobDetail.Key.Name} was executed");
            return Task.CompletedTask;
        }     
    }
}
