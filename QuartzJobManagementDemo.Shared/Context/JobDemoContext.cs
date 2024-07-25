using Microsoft.EntityFrameworkCore;
using QuartzJobManagementDemo.Shared.Entities;

namespace QuartzJobManagementDemo.Shared.Context
{
    public class JobDemoContext : DbContext
    {
        public JobDemoContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Message> Messages { get; set; }
    }
}
