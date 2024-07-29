using Quartz;
using Quartz.Impl.Matchers;
using QuartzJobManagementDemo.Chronos.QuartzJobs;
using QuartzJobManagementDemo.Chronos.Services.Abstract;
using QuartzJobManagementDemo.Shared.Dtos;
using QuartzJobManagementDemo.Shared.Dtos.Job;
using Serilog;

namespace QuartzJobManagementDemo.Chronos.Services.Concrete
{
    public class JobService(ISchedulerFactory schedulerFactory) : IJobService
    {
        private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;

        public async Task<ResponseDto<object>> AddAsync(string name, Dictionary<string, string> parameters, string jobType)
        {
            try
            {
                var scheduler = await _schedulerFactory.GetScheduler();
                IJobDetail? job = null;

                switch (jobType)
                {
                    case "MessagePrinter":
                        job = JobBuilder.Create<MessagePrinterJob>().WithIdentity(name).UsingJobData(new(parameters)).StoreDurably().Build();
                        break;

                    default:
                        break;
                }

                if (job == null) return new("Job type not found.", null, false);

                await scheduler.AddJob(job, false);
                return new("Job added successfully.", null, true);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error occurred while adding job.");
                return new(exception.Message, null, false);
            }
        }

        public async Task<ResponseDto<object>> DeleteAsync(string name)
        {
            try
            {
                var scheduler = await _schedulerFactory.GetScheduler();                
                var response = await scheduler.DeleteJob(new(name));
                return response ? new("Job deleted successfully.", null, true) : new("Job not found.", null, false);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error occurred while deleting job.");
                return new(exception.Message, null, false);
            }
        }

        public async Task<ResponseDto<object>> DeleteJobScheduleAsync(string name)
        {
            try
            {
                var scheduler = await _schedulerFactory.GetScheduler();

                var triggersOfJob = await scheduler.GetTriggersOfJob(new(name));

                foreach (var trigger in triggersOfJob)
                {
                    await scheduler.UnscheduleJob(trigger.Key);
                }

                return new("Job schedule deleted successfully.", null, true);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error occurred while deleting job schedule.");
                return new(exception.Message, null, false);
            }
        }

        public async Task<ResponseDto<IEnumerable<JobDto>>> GetAllAsync()
        {
            try
            {
                var scheduler = await _schedulerFactory.GetScheduler();

                var groupNames = await scheduler.GetJobGroupNames();

                List<JobDto> jobDtos = [];

                foreach (var name in groupNames)
                {
                    var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupContains(name));
                    foreach (var jobKey in jobKeys)
                    {
                        var jobDetail = await scheduler.GetJobDetail(jobKey);

                        if (jobDetail == null) continue;

                        JobDto jobDto = new(
                            jobKey.Name,
                            jobDetail.JobType.Name,
                            jobDetail.JobDataMap["Message"]?.ToString() ?? string.Empty,
                            jobDetail.JobDataMap["CreatedBy"]?.ToString() ?? string.Empty);

                        jobDtos.Add(jobDto);
                    }
                }
                
                return new("Jobs retrieved successfully.", jobDtos, true);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error occurred while getting jobs.");
                return new(exception.Message, null, false);
            }
        }

        public async Task<ResponseDto<IEnumerable<JobScheduleDto>>> GetJobSchedulesAsync()
        {
            try
            {
                var scheduler = await _schedulerFactory.GetScheduler();

                var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

                List<JobScheduleDto> JobScheduleDtos = [];

                foreach (var jobKey in jobKeys)
                {
                    var jobDetail = await scheduler.GetJobDetail(jobKey);

                    if (jobDetail == null) continue;

                    var triggersOfJob = await scheduler.GetTriggersOfJob(jobKey);

                    if (triggersOfJob == null || triggersOfJob.Count == 0) continue;

                    foreach (var trigger in triggersOfJob)
                    {

                        if (trigger is ICronTrigger cronTrigger)
                        {
                            JobScheduleDtos.Add(new(jobKey.Name, cronTrigger.CronExpressionString ?? string.Empty));
                        }
                    }

                }
                
                return new("Job schedules retrieved successfully.", JobScheduleDtos, true);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error occurred while getting job schedules.");                
                return new(exception.Message, null, false);
            }
        }

        public async Task<ResponseDto<object>> ScheduleAsync(string jobName, string cronExpression)
        {
            try
            {
                var scheduler = await _schedulerFactory.GetScheduler();

                IJobDetail? job = await scheduler.GetJobDetail(new(jobName));

                if (job == null) return new("Job not found.", null, false);

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"{jobName}-trigger")
                    .WithCronSchedule(cronExpression)
                    .ForJob(job)
                    .Build();

                await scheduler.ScheduleJob(trigger);

                return new("Job scheduled successfully.", null, true);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error occurred while scheduling job.");
                return new(exception.Message, null, false);
            }

        }
    }
}
