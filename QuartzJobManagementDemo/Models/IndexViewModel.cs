using QuartzJobManagementDemo.Shared.Dtos.Job;

namespace QuartzJobManagementDemo.Models
{
    public class IndexViewModel
    {
        public List<Entities.Message> Messages { get; set; } = [];
        public List<JobDto> CustomJobs { get; set; } = [];
        public List<JobScheduleDto> CustomJobSchedules { get; set; } = [];
    }
}
