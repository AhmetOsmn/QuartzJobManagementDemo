using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using QuartzJobManagementDemo.Chronos;
using QuartzJobManagementDemo.Chronos.Consumers;
using QuartzJobManagementDemo.Chronos.gRPC;
using QuartzJobManagementDemo.Chronos.Services.Abstract;
using QuartzJobManagementDemo.Chronos.Services.Concrete;
using QuartzJobManagementDemo.QuartzJobManagementDemo.Chronos.Context;
using QuartzJobManagementDemo.Shared;

Console.Title = "Chronos";

var database = "Postgres";
var sqlServerConnectionString = "Server=localhost, 1433;Database=QuartzJobManagement.Chronos;User Id=sa;Password=password;TrustServerCertificate=True";
var postgresConnectionString = "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=QuartzJobManagement.Chronos;";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChronosDbContext>(opt =>
{
    if (database == "SqlServer")
        opt.UseSqlServer(sqlServerConnectionString);
    else if (database == "Postgres")
        opt.UseNpgsql(postgresConnectionString);
});


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


var app = builder.Build();

app.MapGrpcService<GrpcJobSOperator>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();
