namespace QuartzJobManagementDemo.Models
{
    public class IndexViewModel
    {
        public List<Entities.Message> Messages { get; set; } = [];
        public List<CustomJob> CustomJobs { get; set; } = [];
        public List<CustomJobSchedule> CustomJobSchedules { get; set; } = [];
    }
}
