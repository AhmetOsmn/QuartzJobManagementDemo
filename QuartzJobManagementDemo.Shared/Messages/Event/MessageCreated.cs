namespace QuartzJobManagementDemo.Shared.Messages.Event
{
    public record MessageCreated(string Message, string Sender, string Receiver, DateTime CreationDate);
}
