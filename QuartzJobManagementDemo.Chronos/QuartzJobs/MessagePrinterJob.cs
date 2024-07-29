using Dapper;
using MassTransit;
using Microsoft.Data.SqlClient;
using Npgsql;
using Quartz;
using QuartzJobManagementDemo.Chronos.Masstransit.Publishers.Interfaces;
using Serilog;
using System.Data;

namespace QuartzJobManagementDemo.Chronos.QuartzJobs
{
    public class MessagePrinterJob(INotificationEventPublisher notificationEventPublisher) : IJob
    {
        private readonly INotificationEventPublisher _notificationEventPublisher = notificationEventPublisher;

        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                try
                {
                    string message = "Message";
                    string createdBy = "CreatedBy";
                    string createdDate = "CreatedDate";
                    string connectionString = "ConnectionString";
                    string dataBase = "Database";

                    var messageData = context.MergedJobDataMap.GetString(message);
                    var createdByData = context.MergedJobDataMap.GetString(createdBy);
                    var createdDateData = context.MergedJobDataMap.GetString(createdDate);
                    var dataBaseData = context.MergedJobDataMap.GetString(dataBase);
                    var connectionStringData = context.MergedJobDataMap.GetString(connectionString);

                    ValidateData(messageData, message);
                    ValidateData(createdByData, createdBy);
                    ValidateData(createdDateData, createdDate);
                    ValidateData(connectionStringData, connectionString);
                    ValidateData(dataBaseData, dataBase);

                    int messageId = 0;

                    if (dataBaseData == "Postgres")
                    {
                        messageId = PostgresInsertMessage(messageData!, createdByData!, DateTime.Parse(createdDateData!).ToUniversalTime(), connectionStringData!);
                    }
                    else
                    {
                        messageId = SqlServerInsertMessage(messageData!, createdByData!, DateTime.Parse(createdDateData!).ToUniversalTime(), connectionStringData!);
                    }

                    _notificationEventPublisher.Publish($"Message created with id:{messageId}, text: {messageData}");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Message Printer Job Failed: {ex.Message}");
                }
            });
        }

        private static void ValidateData(string? data, string dataName)
        {
            if (data == null) throw new ArgumentNullException($"Message Printer Job {dataName} is null");
        }

        private int PostgresInsertMessage(string text, string createdBy, DateTime createdDate, string connectionString)
        {
            int id = 0;
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var newMessage = new
                {
                    Text = text,
                    JobCreatedDate = DateTime.Now.ToUniversalTime(),
                    CreatedBy = createdBy,
                    CreatedDate = createdDate
                };

                string insertQuery = "INSERT INTO \"Messages\" (\"Text\", \"JobCreatedDate\", \"CreatedBy\", \"CreatedDate\") VALUES (@Text, @JobCreatedDate, @CreatedBy, @CreatedDate) RETURNING \"Id\";";
                id = connection.ExecuteScalar<int>(insertQuery, newMessage);
                Log.Information($"Inserted message with ID: {id}");
            }
            return id;
        }

        private int SqlServerInsertMessage(string text, string createdBy, DateTime createdDate, string connectionString)
        {
            int id = 0;

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var newMessage = new
                {
                    Text = text,
                    JobCreatedDate = DateTime.Now.ToUniversalTime(),
                    CreatedBy = createdBy,
                    CreatedDate = createdDate
                };

                string insertQuery = @"
                INSERT INTO Messages (Text, JobCreatedDate, CreatedBy, CreatedDate) 
                VALUES (@Text, @JobCreatedDate, @CreatedBy, @CreatedDate);
                SELECT CAST(SCOPE_IDENTITY() as int);";
                id = connection.ExecuteScalar<int>(insertQuery, newMessage);
                Log.Information($"Inserted message with ID: {id}");
            }
            return id;
        }
    }
}
