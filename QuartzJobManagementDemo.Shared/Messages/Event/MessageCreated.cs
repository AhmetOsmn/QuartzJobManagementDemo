using MassTransit;

namespace QuartzJobManagementDemo.Shared.Messages.Event
{
    [EntityName("MessageCreated")]
    public record MessageCreated(int MessageId, string Message, string Sender, string Receiver);
}
