using Microsoft.AspNetCore.SignalR;

namespace QuartzJobManagementDemo.SignalR.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task Notify(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification",message);
        }
    }
}
