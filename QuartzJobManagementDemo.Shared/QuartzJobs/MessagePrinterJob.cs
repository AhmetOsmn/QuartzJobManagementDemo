using Quartz;
using QuartzJobManagementDemo.Shared.Services.Abstract;

namespace QuartzJobManagementDemo.Shared.QuartzJobs
{
    public class MessagePrinterJob(IMessageService messageService) : IJob
    {
        private readonly IMessageService _messageService = messageService;

        public static readonly JobKey Key = new("message-printer-job", "group1");

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                string message = "Message";
                string createdBy = "CreatedBy";
                string createdDate = "CreatedDate";

                var messageData = context.MergedJobDataMap.GetString(message);
                var createdByData = context.MergedJobDataMap.GetString(createdBy);
                var createdDateData = context.MergedJobDataMap.GetString(createdDate);

                ValidateData(messageData, message);
                ValidateData(createdByData, createdBy);
                ValidateData(createdDateData, createdDate);

                _messageService.Add(message!, DateTime.Parse(createdDate!), createdBy!);

                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                //throw new JobExecutionException(msg: "", refireImmediately: true, cause: ex);
                Console.WriteLine($"Message Printer Job Failed: {ex.Message}");
            }
        }

        private static void ValidateData(string? data, string dataName)
        {
            if (data == null) throw new ArgumentNullException($"Message Printer Job {dataName} is null");
        }
    }
}
