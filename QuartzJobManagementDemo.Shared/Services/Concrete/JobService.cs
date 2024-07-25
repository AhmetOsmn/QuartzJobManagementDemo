using Quartz;
using Quartz.Impl.Matchers;
using QuartzJobManagementDemo.Shared.Models;
using QuartzJobManagementDemo.Shared.QuartzJobs;
using QuartzJobManagementDemo.Shared.Services.Abstract;

namespace QuartzJobManagementDemo.Shared.Services.Concrete
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
                    var job = JobBuilder.Create<MessagePrinterJob>().WithIdentity(jobViewModel.Name).UsingJobData(new(jobViewModel.Parameters)).StoreDurably().Build();
                    await scheduler.AddJob(job, false);
                    break;

                default:
                    break;
            }
        }

        public async Task DeleteAsync(string name)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.DeleteJob(new(name));
        }

        public async Task DeleteJobScheduleAsync(string name)
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var triggersOfJob = await scheduler.GetTriggersOfJob(new(name));

            foreach (var trigger in triggersOfJob)
            {
                await scheduler.UnscheduleJob(trigger.Key);
            }
        }

        public async Task<List<CustomJob>> GetAllAsync()
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var groupNames = await scheduler.GetJobGroupNames();

            List<CustomJob> customJobs = [];

            foreach (var name in groupNames)
            {
                var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupContains(name));
                foreach (var jobKey in jobKeys)
                {
                    var jobDetail = await scheduler.GetJobDetail(jobKey);

                    if (jobDetail == null) continue;

                    CustomJob customJob = new()
                    {
                        Name = jobKey.Name,
                        CreatedBy = jobDetail.JobDataMap["CreatedBy"]?.ToString() ?? string.Empty,
                        Text = jobDetail.JobDataMap["Message"]?.ToString() ?? string.Empty,
                        Type = jobDetail.JobType.Name,
                    };
                    customJobs.Add(customJob);
                }
            }
            return customJobs;
        }

        public async Task<List<CustomJobSchedule>> GetJobSchedulesAsync()
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            List<CustomJobSchedule> customJobs = [];

            foreach (var jobKey in jobKeys)
            {
                var jobDetail = await scheduler.GetJobDetail(jobKey);

                if (jobDetail == null) continue;

                var triggersOfJob = await scheduler.GetTriggersOfJob(jobKey);

                if (triggersOfJob == null || triggersOfJob.Count == 0) continue;

                foreach (var trigger in triggersOfJob)
                {

                    if (trigger is ICronTrigger)
                    {
                        customJobs.Add(new() { Name = jobKey.Name, CRON = ((ICronTrigger)trigger).CronExpressionString });
                    }
                }

            }

            return customJobs;
        }

        public async Task ScheduleAsync(string jobName, string cronExpression)
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            IJobDetail? job = await scheduler.GetJobDetail(new(jobName)) ?? throw new InvalidOperationException("Job not found."); ;

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobName}-trigger")
                .WithCronSchedule(cronExpression)
                .ForJob(job)
                .Build();

            await scheduler.ScheduleJob(trigger);
        }
    }
}
