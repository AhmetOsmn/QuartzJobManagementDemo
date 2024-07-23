using Quartz;
using QuartzJobManagementDemo.Configuration;
using QuartzJobManagementDemo.Models;
using QuartzJobManagementDemo.QuartzJobs;
using QuartzJobManagementDemo.Services.Abstract;

namespace QuartzJobManagementDemo.Services.Concrete
{
    public class JobService(ISchedulerFactory schedulerFactory) : IJobService
    {
        private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;

        public async Task AddAsync(JobViewModel jobViewModel)
        {
            switch (jobViewModel.Type)
            {
                case "MessagePrinter":                    
                    var scheduler = await _schedulerFactory.GetScheduler();
                    var job = JobBuilder.Create<MessagePrinterJob>().WithIdentity(jobViewModel.Name).StoreDurably().Build();         
                    await scheduler.AddJob(job, false);
                    break;

                default:
                    break;
            }
        }

        public void Delete(string id)
        {

        }

        public void DeleteJobSchedule(string id)
        {

        }

        public List<CustomJob> GetAll()
        {
            return new();
        }

        public List<CustomJobSchedule> GetJobSchedules()
        {
            return new();
        }

        public void Schedule(string jobId, string cronExpression)
        {

        }

        private T GetRecurringJobParameters<T>(string jobId) where T : class
        {
            throw new Exception();
        }
    }
}
