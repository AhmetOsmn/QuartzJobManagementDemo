namespace QuartzJobManagementDemo.Chronos.Services.Abstract
{
    public interface IMessageCreatedEventService
    {
        Task AddAsync(int messageId, string message, string sender, string receiver);
        Task DeleteAsync(int id);
        Task DeleteAllAsync();
    }
}
