using Quartz;
using QuartzJobManagementDemo.Services.Abstract;

namespace QuartzJobManagementDemo.QuartzJobs
{
    public class MessagePrinterJob(IMessageService messageService) : IJob
    {
        private readonly IMessageService _messageService = messageService;
        
        public static readonly JobKey Key = new("message-printer-job", "group1");

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                //var message = context.MergedJobDataMap.GetString("message");
                Console.WriteLine($"message-printer-job");
                
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                //throw new JobExecutionException(msg: "", refireImmediately: true, cause: ex);
                Console.WriteLine($"Message Printer Job Failed: {ex.Message}");
            }
        }
    }
}
