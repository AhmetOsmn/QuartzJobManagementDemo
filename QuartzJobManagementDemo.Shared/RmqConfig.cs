namespace QuartzJobManagementDemo.Shared
{
    public class RmqConfig
    {
        public const string RmqUri = "rabbitmq://localhost/";
        public const string RmqUserName = "guest";
        public const string RmqPassword = "guest";
        public const string NotificationQueueName = "QuartzJobManagementDemo.Notification";
    }
}
