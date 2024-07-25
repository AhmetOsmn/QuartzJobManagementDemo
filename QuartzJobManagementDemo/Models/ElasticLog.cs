namespace QuartzJobManagementDemo.Models
{
    public class ElasticLog
    {
        public object? Data { get; set; }
        public DateTime LogDate{ get; set; }
        public required string Username { get; set; }
        public required string ApplicationName { get; set; }
    }
}
