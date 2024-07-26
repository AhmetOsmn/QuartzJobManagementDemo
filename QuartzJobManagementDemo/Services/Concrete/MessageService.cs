using QuartzJobManagementDemo.Context;
using QuartzJobManagementDemo.Entities;
using QuartzJobManagementDemo.Services.Abstract;

namespace QuartzJobManagementDemo.Services.Concrete
{
    public class MessageService(JobDemoContext dbContext) : IMessageService
    {
        private readonly JobDemoContext _dbContext = dbContext;

        public void Add(string text, DateTime? jobCreatedDate, string createdBy)
        {
            _dbContext.Add(new Message { Text = text, CreatedBy = createdBy, JobCreatedDate = jobCreatedDate.HasValue ? jobCreatedDate.Value.ToUniversalTime() : null, CreatedDate = DateTime.Now.ToUniversalTime() });
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var selectedMessage = _dbContext.Messages.Find(id);

            ArgumentNullException.ThrowIfNull(selectedMessage);

            _dbContext.Messages.Remove(selectedMessage);
            _dbContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _dbContext.RemoveRange(_dbContext.Messages);
            _dbContext.SaveChanges();
        }

        public List<Message> GetAll()
        {
            return _dbContext.Messages.ToList();
        }
    }
}
