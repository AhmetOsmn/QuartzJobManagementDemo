using Microsoft.EntityFrameworkCore;
using QuartzJobManagementDemo.Entities;

namespace QuartzJobManagementDemo.Context
{
    public class JobDemoContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<Message> Messages { get; set; }
    }
}
