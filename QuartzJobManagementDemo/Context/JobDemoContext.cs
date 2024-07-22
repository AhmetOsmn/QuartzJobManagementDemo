
using Microsoft.EntityFrameworkCore;
using QuartzJobManagementDemo.Entities;

namespace QuartzJobManagementDemo.Context
{
    public class JobDemoContext : DbContext
    {
        public JobDemoContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Message> Messages { get; set; }
    }
}
