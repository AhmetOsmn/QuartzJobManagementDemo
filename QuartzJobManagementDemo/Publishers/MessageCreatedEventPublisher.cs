using MassTransit;
using QuartzJobManagementDemo.Shared;
using QuartzJobManagementDemo.Shared.Messages.Event;
using Serilog;

namespace QuartzJobManagementDemo.Publishers
{
    public class MessageCreatedEventPublisher(IBus bus) : IMessageCreatedEventPublisher
    {
        private readonly IBus _bus = bus;

        public async Task Publish(int messageId, string message, string sender, string receiver)
        {            
            await _bus.Publish(new MessageCreated(messageId, message, sender, receiver));
            Log.Information("MessageCreatedEvent published with MessageId: {MessageId}", messageId);
            await Task.Delay(100);
        }
    }
}
