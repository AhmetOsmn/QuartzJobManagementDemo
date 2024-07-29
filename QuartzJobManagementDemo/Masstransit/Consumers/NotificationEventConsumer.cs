using MassTransit;
using Microsoft.AspNetCore.SignalR;
using QuartzJobManagementDemo.Shared.Messages.Event;
using QuartzJobManagementDemo.SignalR.Hubs;

namespace QuartzJobManagementDemo.Masstransit.Consumers
{
    public class NotificationEventConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<Notification>
    {
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;

        public async Task Consume(ConsumeContext<Notification> context)
        {            
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", context.Message.Message);
        }
    }
}
