using Quartz;
using QuartzJobManagementDemo.Shared.Dtos;

namespace QuartzJobManagementDemo.Chronos.Services.Abstract
{
    public interface IJobService
    {
        Task<ResponseDto> AddAsync(string name, Dictionary<string, string> parameters, Type jobType);
        Task<ResponseDto> ScheduleAsync(string jobName, string cronExpression);
        Task<ResponseDto> DeleteAsync(string name);
        Task<ResponseDto> DeleteJobScheduleAsync(string name);
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto> GetJobSchedulesAsync();
    }
}
