using ResponseDto = QuartzJobManagementDemo.Shared.Dtos.ResponseDto;

namespace QuartzJobManagementDemo.Services.Abstract
{
    public interface IJobService
    {
        Task<ResponseDto> AddAsync(string name, Dictionary<string, string> parameters);
        Task<ResponseDto> DeleteAsync(string name);
        Task<ResponseDto> DeleteJobScheduleAsync(string name);
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto> GetJobSchedulesAsync();
        Task<ResponseDto> ScheduleAsync(string jobName, string cronExpression);
    }
}
