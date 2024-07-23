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
                var message = context.MergedJobDataMap.GetString(MessagePrinterJobParameters.Message);
                var createdBy = context.MergedJobDataMap.GetString(MessagePrinterJobParameters.CreatedBy);
                var createdDate = context.MergedJobDataMap.GetString(MessagePrinterJobParameters.CreatedDate);

                ValidateData(message, MessagePrinterJobParameters.Message);
                ValidateData(createdBy, MessagePrinterJobParameters.CreatedBy);
                ValidateData(createdDate, MessagePrinterJobParameters.CreatedDate);

                createdBy = $"{createdBy}:{Key.Name}";

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
