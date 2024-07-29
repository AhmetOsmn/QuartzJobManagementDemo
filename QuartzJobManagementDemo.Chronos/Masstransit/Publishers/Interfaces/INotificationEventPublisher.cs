namespace QuartzJobManagementDemo.Chronos.Masstransit.Publishers.Interfaces
{
    public interface INotificationEventPublisher
    {
        Task Publish(string message);
    }
}
