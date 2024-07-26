using MassTransit;
using QuartzJobManagementDemo.Chronos.Services.Abstract;
using QuartzJobManagementDemo.Chronos.Services.Concrete;
using QuartzJobManagementDemo.Shared.Messages.Event;

namespace QuartzJobManagementDemo.Chronos.Consumers
{
    public class MessageCreatedEventConsumer(IMessageCreatedEventService messageCreatedEventService) : IConsumer<MessageCreated>
    {
        private readonly IMessageCreatedEventService _messageCreatedEventService = messageCreatedEventService;

        public async Task Consume(ConsumeContext<MessageCreated> context)
        {
            var message = context.Message;

            await _messageCreatedEventService.AddAsync(message.MessageId, message.Message, message.Sender, message.Receiver); 

            await Task.Delay(5000);
        }
    }
}
