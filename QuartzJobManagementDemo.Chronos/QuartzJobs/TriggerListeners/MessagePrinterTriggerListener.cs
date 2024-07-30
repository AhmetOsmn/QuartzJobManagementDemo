using Quartz;
using Serilog;

namespace QuartzJobManagementDemo.Chronos.QuartzJobs.TriggerListeners
{
    public class MessagePrinterTriggerListener : ITriggerListener
    {
        public string Name => "MessagePrinterTriggerListener";

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
        {
            Log.Information($"-------------Trigger: {trigger.Key.Name} completed");
            return Task.CompletedTask;
        }

        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Log.Information($"-------------Trigger: {trigger.Key.Name} fired");
            return Task.CompletedTask;            
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Log.Information($"-------------Trigger: {trigger.Key.Name} misfired");
            return Task.CompletedTask;
        }

        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {            
            return Task.FromResult(IsDivisibleBy7());
        }

        private static bool IsDivisibleBy7()
        {
            Random random = new();
            int randomNumber = random.Next(1, 10);
            if (randomNumber % 7 == 0) return true;
            return false;
        }
    }
}
