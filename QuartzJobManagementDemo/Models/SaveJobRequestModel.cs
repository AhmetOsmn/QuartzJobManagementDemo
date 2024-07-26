namespace QuartzJobManagementDemo.Models
{
    public class SaveJobRequestModel
    {
        public string JobName { get; set; } = null!;
        public string JobType { get; set; } = null!;
        public string? MessageText { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
