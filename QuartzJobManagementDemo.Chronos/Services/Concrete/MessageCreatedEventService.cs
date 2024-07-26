using Microsoft.EntityFrameworkCore;
using QuartzJobManagementDemo.Chronos.Entities;
using QuartzJobManagementDemo.Chronos.Services.Abstract;
using QuartzJobManagementDemo.QuartzJobManagementDemo.Chronos.Context;

namespace QuartzJobManagementDemo.Chronos.Services.Concrete
{
    public class MessageCreatedEventService(ChronosDbContext dbContext) : IMessageCreatedEventService
    {
        private readonly ChronosDbContext _dbContext = dbContext;

        public async Task AddAsync(int messageId, string message, string sender, string receiver)
        {
            _dbContext.Add(new MessageCreatedEvent { MessageId = messageId, Message = message, Sender = sender, Receiver = receiver });
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var selectedMessage = _dbContext.MessageCreatedEvents.Find(id);

            ArgumentNullException.ThrowIfNull(selectedMessage);

            _dbContext.MessageCreatedEvents.Remove(selectedMessage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllAsync()
        {
            _dbContext.RemoveRange(_dbContext.MessageCreatedEvents);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<MessageCreatedEvent>> GetAllAsync()
        {
            return await _dbContext.MessageCreatedEvents.AsNoTracking().ToListAsync();
        }
    }
}
