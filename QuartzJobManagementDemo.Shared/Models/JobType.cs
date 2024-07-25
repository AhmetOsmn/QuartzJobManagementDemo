namespace QuartzJobManagementDemo.Shared.Models
{
    public static class JobType
    {
        public static TextValue MessagePrinter => new("Message Printer", "MessagePrinter");
        public static TextValue MailSender => new("Mail Sender", "MailSender");
        public static TextValue Cleaner => new("Cleaner", "Cleaner");
    }
}
