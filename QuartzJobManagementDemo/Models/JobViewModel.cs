namespace QuartzJobManagementDemo.Models
{
    public class JobViewModel
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public Dictionary<string, string> Parameters { get; set; } = new();
    }
}
