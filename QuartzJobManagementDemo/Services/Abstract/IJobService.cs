﻿using QuartzJobManagementDemo.Models;

namespace QuartzJobManagementDemo.Services.Abstract
{
    public interface IJobService
    {
        Task AddAsync(JobViewModel jobViewModel);
        Task ScheduleAsync(string jobName, string cronExpression);
        Task DeleteAsync(string name);
        Task DeleteJobScheduleAsync(string name);
        Task<List<CustomJob>> GetAllAsync();
        Task<List<CustomJobSchedule>> GetJobSchedulesAsync();
    }
}
