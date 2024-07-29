using Chronos.Server;
using Grpc.Core;
using QuartzJobManagementDemo.Chronos.Services.Abstract;
using Empty = Chronos.Server.Empty;


namespace QuartzJobManagementDemo.Chronos.gRPC
{
    public class GrpcJobSOperator(IJobService jobService) : GrpcJobService.GrpcJobServiceBase
    {
        private readonly IJobService _jobService = jobService;

        public override async Task<Bool> AddAsync(AddRequest request, ServerCallContext context)
        {
            var parameters = new Dictionary<string, string>();

            foreach (var parameter in request.Parameters.Pairs)
            {
                parameters.Add(parameter.Key, parameter.Value);
            }

            var response = await _jobService.AddAsync(request.Name, parameters, request.JobType);

            return new() { Status = response.Success };
        }

        public override async Task<GetJobSchedulesResponse> GetJobSchedulesAsync(Empty request, ServerCallContext context)
        {
            var response = await _jobService.GetJobSchedulesAsync();

            var result = new GetJobSchedulesResponse();

            if (response.Success && response.Data != null)
            {
                foreach (var item in response.Data)
                {
                    result.JobSchedules.Add(new JobScheduleDto() { Name = item.Name, CronExpression = item.Cron });
                }
            }

            return result;
        }

        public override async Task<Bool> DeleteAsync(DeleteRequest request, ServerCallContext context)
        {
            var response = await _jobService.DeleteAsync(request.Name);

            return new Bool() { Status = response.Success };
        }

        public override async Task<Bool> DeleteJobScheduleAsync(DeleteJobScheduleRequest request, ServerCallContext context)
        {
            var response = await _jobService.DeleteJobScheduleAsync(request.Name);

            return new Bool() { Status = response.Success };
        }

        public override async Task<GetAllResponse> GetAllAsync(Empty request, ServerCallContext context)
        {
            GetAllResponse result = new();

            var response = await _jobService.GetAllAsync();

            if (response.Success && response.Data != null)
            {
                foreach (var item in response.Data)
                {
                    result.Jobs.Add(new JobDto() { CreatedBy = item.CreatedBy, Message = item.Message, Name = item.Name, Type = item.Type });
                }
            }

            return result;
        }

        public override async Task<Bool> ScheduleAsync(ScheduleRequest request, ServerCallContext context)
        {
            var response = await _jobService.ScheduleAsync(request.JobName, request.CronExpression);

            return new Bool() { Status = response.Success };
        }
    }
}
