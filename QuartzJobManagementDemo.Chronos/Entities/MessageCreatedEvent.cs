namespace QuartzJobManagementDemo.Chronos.Entities
{
    public class MessageCreatedEvent
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Message { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Receiver { get; set; }= null!;
    }
}
