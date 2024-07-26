namespace QuartzJobManagementDemo.Shared.Dtos.Job
{
    public record AddJobDto(string Name, Dictionary<string, string> Parameters);
}
