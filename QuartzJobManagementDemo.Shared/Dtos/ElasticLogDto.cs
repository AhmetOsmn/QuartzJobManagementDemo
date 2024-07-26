namespace QuartzJobManagementDemo.Shared.Dtos
{
    public class ElasticLogDto
    {
        public object? Data { get; set; }
        public DateTime LogDate { get; set; }
        public required string Username { get; set; }
        public required string ApplicationName { get; set; }
    }
}
