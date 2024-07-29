using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using QuartzJobManagementDemo.Chronos;
using QuartzJobManagementDemo.Chronos.gRPC;
using QuartzJobManagementDemo.Chronos.Masstransit.Consumers;
using QuartzJobManagementDemo.Chronos.Masstransit.Publishers;
using QuartzJobManagementDemo.Chronos.Masstransit.Publishers.Interfaces;
using QuartzJobManagementDemo.Chronos.Services.Abstract;
using QuartzJobManagementDemo.Chronos.Services.Concrete;
using QuartzJobManagementDemo.QuartzJobManagementDemo.Chronos.Context;
using QuartzJobManagementDemo.Shared;
using QuartzJobManagementDemo.Shared.Extensions;

Console.Title = "Chronos";


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

var database = configuration["Database"];
var sqlServerConnectionString = configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Sql Server connection string is null or empty.");
var postgresConnectionString = configuration.GetConnectionString("Postgres") ?? throw new InvalidOperationException("Postgres connection string is null or empty");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000, o => o.Protocols =
        HttpProtocols.Http2);
});

builder.Services.AddDbContext<ChronosDbContext>(opt =>
{
    if (database == "SqlServer")
        opt.UseSqlServer(sqlServerConnectionString);
    else if (database == "Postgres")
        opt.UseNpgsql(postgresConnectionString);
});

builder.Services.AddScoped<IMessageCreatedEventPublisher, MessageCreatedEventPublisher>();
builder.Services.AddScoped<INotificationEventPublisher, NotificationEventPublisher>();
builder.Services.AddScoped<IMessageCreatedEventService, MessageCreatedEventService>();
builder.Services.AddScoped<IJobService, JobService>();

builder.Services.AddGrpc();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MessageCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(RmqConfig.RmqUri), hst =>
        {
            hst.Username(RmqConfig.RmqUserName);
            hst.Password(RmqConfig.RmqPassword);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHostedService<MassTransitConsoleHostedService>();

builder.Services.AddQuartz(cfg =>
{
    cfg.UsePersistentStore(store =>
    {
        store.UseProperties = true;
        store.UseSystemTextJsonSerializer();

        if (database == "SqlServer")
            store.UseSqlServer(sqlServerConnectionString);

        else if (database == "Postgres")
            store.UsePostgres(postgresConnectionString);
    });
});
builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});

builder.UseMySerilog();

var app = builder.Build();

app.MapGrpcService<GrpcJobSOperator>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();
