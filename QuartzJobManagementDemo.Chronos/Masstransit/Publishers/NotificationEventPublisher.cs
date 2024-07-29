using MassTransit;
using QuartzJobManagementDemo.Chronos.Masstransit.Publishers.Interfaces;
using QuartzJobManagementDemo.Shared.Messages.Event;
using Serilog;

namespace QuartzJobManagementDemo.Chronos.Masstransit.Publishers
{
    public class NotificationEventPublisher(IBus bus) : INotificationEventPublisher
    {
        private readonly IBus _bus = bus;

        public async Task Publish(string message)
        {
            await _bus.Publish(new Notification(message));
            Log.Information("MessageCreatedEvent published with Message: {message}", message);
        }
    }
}
