namespace QuartzJobManagementDemo.Chronos.Entities
{
    public class NotificationSentEvent
    {
        public int Id { get; set; }
        public string User { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
