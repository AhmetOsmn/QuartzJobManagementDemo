using Chronos.Client;
using Google.Protobuf.WellKnownTypes;
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

        public async Task<ResponseDto<object>> AddAsync(string name, Dictionary<string, string> parameters)
        {
            Dictionary dictionaryParameters = new();

            foreach (var item in parameters)
            {
                dictionaryParameters.Pairs.Add(new Pair() { Key = item.Key, Value = item.Value });
            }

            var response = await _client.AddAsyncAsync(new() { Name = name, Parameters = dictionaryParameters });
            return new(response.Message, response.Data, response.Success);
        }

        public async Task<ResponseDto<object>> DeleteAsync(string name)
        {
            var response = await _client.DeleteAsyncAsync(new() { Name = name });
            return new(response.Message, response.Data, response.Success);
        }

        public async Task<ResponseDto<object>> DeleteJobScheduleAsync(string name)
        {
            var response = await _client.DeleteJobScheduleAsyncAsync(new() { Name = name });
            return new(response.Message, response.Data, response.Success);
        }

        public async Task<ResponseDto<IEnumerable<JobDto>>> GetAllAsync()
        {
            var response = await _client.GetAllAsyncAsync(new());
            return new(response.Message, response.Data, response.Success);
        }

        public async Task<ResponseDto<IEnumerable<JobScheduleDto>>> GetJobSchedulesAsync()
        {
            var response = await _client.GetJobSchedulesAsyncAsync(new());

            var temp = response.Data.Unpack(JobSchedule);

            return new(response.Message, response.Data, response.Success);
        }

        public async Task<ResponseDto<object>> ScheduleAsync(string jobName, string cronExpression)
        {
            var response = await _client.ScheduleAsyncAsync(new() { JobName = jobName, CronExpression = cronExpression });
            return new(response.Message, response.Data, response.Success);
        }
    }
}
