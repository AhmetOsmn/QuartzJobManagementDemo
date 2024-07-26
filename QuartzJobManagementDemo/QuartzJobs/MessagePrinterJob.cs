using Quartz;
using QuartzJobManagementDemo.Publishers;
using QuartzJobManagementDemo.Services.Abstract;
using Serilog;

namespace QuartzJobManagementDemo.QuartzJobs
{
    public class MessagePrinterJob(IMessageService messageService, IMessageCreatedEventPublisher messageCreatedEventPublisher) : IJob
    {
        private readonly IMessageService _messageService = messageService;
        private readonly IMessageCreatedEventPublisher _messageCreatedEventPublisher = messageCreatedEventPublisher;
        
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

                _messageService.Add(messageData!, DateTime.Parse(createdDateData!), createdByData!);

                await _messageCreatedEventPublisher.Publish(1, messageData!, "sender1", "receiver1");

                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                Log.Error($"Message Printer Job Failed: {ex.Message}");
            }
        }

        private static void ValidateData(string? data, string dataName)
        {
            if (data == null) throw new ArgumentNullException($"Message Printer Job {dataName} is null");
        }
    }
}
