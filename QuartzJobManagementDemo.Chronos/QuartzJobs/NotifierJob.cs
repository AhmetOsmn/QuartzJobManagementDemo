using MassTransit;
using Quartz;
using QuartzJobManagementDemo.Chronos.Services;
using QuartzJobManagementDemo.Chronos.Services.Abstract;
using QuartzJobManagementDemo.Shared.Messages.Event;

namespace QuartzJobManagementDemo.Chronos.QuartzJobs
{
    public class NotifierJob(IBus bus,IMessageCreatedEventService messageCreatedEventService) : IJob
    {
        private readonly IBus _bus = bus;
        private readonly IMessageCreatedEventService _messageCreatedEventService = messageCreatedEventService;

        public static readonly JobKey Key = new("notifier-job");

        public async Task Execute(IJobExecutionContext context)
        {
            var user = "test user";
            var message = "test message";

            NotifyService.Notify(user, message);
            
            await _bus.Publish<Notification>(new(message));
        }
    }
}
