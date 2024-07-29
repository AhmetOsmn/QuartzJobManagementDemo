using Chronos.Client;
using Grpc.Net.Client;
using QuartzJobManagementDemo.Services.Abstract;
using QuartzJobManagementDemo.Shared.Dtos;
using QuartzJobManagementDemo.Shared.Dtos.Job;

namespace QuartzJobManagementDemo.Services.Concrete
{
    public class JobService : IJobService
    {
        private readonly GrpcJobService.GrpcJobServiceClient _client;

        public JobService(IConfiguration configuration)
        {
            var grpcServerUrl = configuration.GetSection("gRPC:chronos:Url").Value
                ?? throw new InvalidOperationException("GrpcServerUrl is null or empty.");

            var channel = GrpcChannel.ForAddress(grpcServerUrl);
            _client = new GrpcJobService.GrpcJobServiceClient(channel);
        }

        public async Task<ResponseDto<object>> AddAsync(string name, Dictionary<string, string> parameters, string jobType)
        {
            Dictionary dictionaryParameters = new();

            foreach (var item in parameters)
            {
                dictionaryParameters.Pairs.Add(new Pair() { Key = item.Key, Value = item.Value });
            }

            var response = await _client.AddAsyncAsync(new() { Name = name, Parameters = dictionaryParameters, JobType = jobType });

            return new(response.Status ? "Add operation success." : "Add operation failed.", null, response.Status);
        }

        public async Task<ResponseDto<object>> DeleteAsync(string name)
        {
            var response = await _client.DeleteAsyncAsync(new() { Name = name });

            return new(response.Status ? "Delete operation success." : "Delete operation failed.", null, response.Status);
        }

        public async Task<ResponseDto<object>> DeleteJobScheduleAsync(string name)
        {
            var response = await _client.DeleteJobScheduleAsyncAsync(new() { Name = name });
            return new(response.Status ? "Delete job schedule operation success." : "Delete job schedule operation failed.", null, response.Status);
        }

        public async Task<ResponseDto<IEnumerable<Shared.Dtos.Job.JobDto>>> GetAllAsync()
        {
            var response = await _client.GetAllAsyncAsync(new());

            var jobDtos = new List<Shared.Dtos.Job.JobDto>();

            foreach (var item in response.Jobs)
            {
                jobDtos.Add(new(item.Name, item.Type, item.Message, item.CreatedBy));
            }

            var message = jobDtos.Count > 0 ? "Get all operation success." : "No job found.";
            return new(message, jobDtos, true);
        }

        public async Task<ResponseDto<IEnumerable<Shared.Dtos.Job.JobScheduleDto>>> GetJobSchedulesAsync()
        {
            var response = await _client.GetJobSchedulesAsyncAsync(new());

            var jobScheduleDtos = new List<Shared.Dtos.Job.JobScheduleDto>();

            foreach (var item in response.JobSchedules)
            {
                jobScheduleDtos.Add(new(item.Name, item.CronExpression));
            }

            var message = jobScheduleDtos.Count > 0 ? "Get all job schedule operation success." : "No job schedule found.";
            return new(message, jobScheduleDtos, true);
        }

        public async Task<ResponseDto<object>> ScheduleAsync(string jobName, string cronExpression)
        {
            var response = await _client.ScheduleAsyncAsync(new() { JobName = jobName, CronExpression = cronExpression });

            return new(response.Status ? "Schedule operation success." : "Schedule operation failed.", null, response.Status);            
        }
    }
}
