namespace QuartzJobManagementDemo.Publishers
{
    public interface IMessageCreatedEventPublisher
    {
        Task Publish(int messageId, string message, string sender, string receiver);
    }
}
