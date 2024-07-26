using Chronos.Server;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Quartz;
using QuartzJobManagementDemo.Chronos.Services.Abstract;
using Empty = Chronos.Server.Empty;


namespace QuartzJobManagementDemo.Chronos.gRPC
{
    public class GrpcJobSOperator(IJobService jobService) : GrpcJobService.GrpcJobServiceBase
    {
        private readonly IJobService _jobService = jobService;

        public override async Task<ResponseDto> AddAsync(AddRequest request, ServerCallContext context)
        {
            var temp = request.JobType.GetType();

            System.Type jobType = System.Type.GetType(request.JobType.ToString());

            if (jobType == null || !typeof(IJob).IsAssignableFrom(jobType))
            {
                return new ResponseDto { Message = "Invalid job type.", Success = false };
            }

            Dictionary<string, string> parameters = new();
            foreach (var item in request.Parameters.Pairs)
            {
                parameters.Add(item.Key, item.Value);
            }

            var response = await _jobService.AddAsync(request.Name, parameters, jobType);

            return new()
            {
                Data = ConvertToGrpcAny(response.Data),
                Message = response.Message,
                Success = response.Success
            };
        }

        public override async Task<ResponseDto> DeleteAsync(DeleteRequest request, ServerCallContext context)
        {
            var response = await _jobService.DeleteAsync(request.Name);

            return new()
            {
                Data = ConvertToGrpcAny(response.Data),
                Message = response.Message,
                Success = response.Success
            };
        }

        public override async Task<ResponseDto> DeleteJobScheduleAsync(DeleteJobScheduleRequest request, ServerCallContext context)
        {
            var response = await _jobService.DeleteJobScheduleAsync(request.Name);

            return new()
            {
                Data = ConvertToGrpcAny(response.Data),
                Message = response.Message,
                Success = response.Success
            };
        }

        public override async Task<ResponseDto> GetAllAsync(Empty request, ServerCallContext context)
        {
            var response = await _jobService.GetAllAsync();

            return new()
            {
                Data = ConvertToGrpcAny(response.Data),
                Message = response.Message,
                Success = response.Success
            };
        }

        public override async Task<ResponseDto> GetJobSchedulesAsync(Empty request, ServerCallContext context)
        {
            var response = await _jobService.GetJobSchedulesAsync();

            return new()
            {
                Data = ConvertToGrpcAny(response.Data),
                Message = response.Message,
                Success = response.Success
            };
        }

        public override async Task<ResponseDto> ScheduleAsync(ScheduleRequest request, ServerCallContext context)
        {
            var response = await _jobService.ScheduleAsync(request.JobName, request.CronExpression);
            return new()
            {
                Data = ConvertToGrpcAny(response.Data),
                Message = response.Message,
                Success = response.Success
            };
        }

        private Any? ConvertToGrpcAny(object? data) => data != null ? Any.Pack((IMessage)data) : null;
    }
}
