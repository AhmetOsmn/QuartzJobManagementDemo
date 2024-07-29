namespace QuartzJobManagementDemo.Shared.Dtos
{
    public record ResponseDto<T>(string Message, T? Data, bool Success);
}
