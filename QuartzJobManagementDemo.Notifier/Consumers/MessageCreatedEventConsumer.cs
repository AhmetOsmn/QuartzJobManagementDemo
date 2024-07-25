using MassTransit;
using QuartzJobManagementDemo.Shared.Messages.Event;

namespace QuartzJobManagementDemo.Notifier.Consumers
{
    public class MessageCreatedEventConsumer : IConsumer<MessageCreated>
    {
        public async Task Consume(ConsumeContext<MessageCreated> context)
        {
            await Console.Out.WriteLineAsync($"Received Message: '{context.Message.Message}' from {context.Message.Sender} to {context.Message.Receiver}");
            // Send email, notificatin, SMS, etc.
            // publish 
        }
    }
}
