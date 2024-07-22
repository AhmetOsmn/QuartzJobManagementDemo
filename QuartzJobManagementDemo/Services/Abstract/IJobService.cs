using QuartzJobManagementDemo.Models;

namespace QuartzJobManagementDemo.Services.Abstract
{
    public interface IJobService
    {
        void Add(JobViewModel jobViewModel);
        void Schedule(string jobId, string cronExpression);
        void Delete(string id);
        void DeleteJobSchedule(string id);
        List<CustomJob> GetAll();
        List<CustomJobSchedule> GetJobSchedules();
    }
}
