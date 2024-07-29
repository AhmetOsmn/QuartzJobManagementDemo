using QuartzJobManagementDemo.Shared.Dtos;
using QuartzJobManagementDemo.Shared.Dtos.Job;

namespace QuartzJobManagementDemo.Chronos.Services.Abstract
{
    public interface IJobService
    {
        Task<ResponseDto<object>> AddAsync(string name, Dictionary<string, string> parameters, string jobType);
        Task<ResponseDto<object>> ScheduleAsync(string jobName, string cronExpression);
        Task<ResponseDto<object>> DeleteAsync(string name);
        Task<ResponseDto<object>> DeleteJobScheduleAsync(string name);
        Task<ResponseDto<IEnumerable<JobDto>>> GetAllAsync();
        Task<ResponseDto<IEnumerable<JobScheduleDto>>> GetJobSchedulesAsync();
    }
}
