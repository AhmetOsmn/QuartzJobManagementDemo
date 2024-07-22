using QuartzJobManagementDemo.Models;
using QuartzJobManagementDemo.Services.Abstract;

namespace QuartzJobManagementDemo.Services.Concrete
{
    public class JobService : IJobService
    {
        public void Add(JobViewModel jobViewModel)
        {
            switch (jobViewModel.Type)
            {
                case "MessagePrinter":
                    break;

                default:
                    break;
            }
        }

        public void Delete(string id)
        {
            throw new Exception();
        }

        public void DeleteJobSchedule(string id)
        {
            throw new Exception();
        }

        public List<CustomJob> GetAll()
        {
            throw new Exception();
        }

        public List<CustomJobSchedule> GetJobSchedules()
        {
            throw new Exception();
        }

        public void Schedule(string jobId, string cronExpression)
        {
            throw new Exception();
        }

        private T GetRecurringJobParameters<T>(string jobId) where T : class
        {
            throw new Exception();
        }
    }
}
