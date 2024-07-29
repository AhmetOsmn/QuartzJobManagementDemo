namespace QuartzJobManagementDemo.Chronos.Masstransit.Publishers.Interfaces
{
    public interface IMessageCreatedEventPublisher
    {
        Task Publish(int messageId, string message, string sender, string receiver);
    }
}
