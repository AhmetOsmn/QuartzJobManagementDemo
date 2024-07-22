using QuartzJobManagementDemo.Entities;

namespace QuartzJobManagementDemo.Services.Abstract
{
    public interface IMessageService
    {
        void Add(string text, DateTime? jobCreatedDate, string createdBy);
        void Delete(int id);
        List<Message> GetAll();
    }
}
