using Microsoft.EntityFrameworkCore;
using QuartzJobManagementDemo.Chronos.Entities;

namespace QuartzJobManagementDemo.QuartzJobManagementDemo.Chronos.Context
{
    public class ChronosDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<NotificationSentEvent> NotificationSentEvents { get; set; }
        public DbSet<MessageCreatedEvent> MessageCreatedEvents { get; set; }
    }
}
