using QuartzJobManagementDemo.Shared.Entities;

namespace QuartzJobManagementDemo.Shared.Services.Abstract
{
    public interface IMessageService
    {
        void Add(string text, DateTime? jobCreatedDate, string createdBy);
        void Delete(int id);
        void DeleteAll();
        List<Message> GetAll();
    }
}
