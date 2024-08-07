﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;
using QuartzJobManagementDemo.Context;
using QuartzJobManagementDemo.Masstransit.Consumers;
using QuartzJobManagementDemo.Services.Abstract;
using QuartzJobManagementDemo.Services.Concrete;
using QuartzJobManagementDemo.Shared;
using QuartzJobManagementDemo.Shared.Extensions;
using QuartzJobManagementDemo.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

var database = builder.Configuration.GetSection("Database").Value ?? throw new InvalidOperationException("Database is null or empty.");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<JobDemoContext>(opt =>
{
    if (database == "SqlServer")
        opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    else if (database == "Postgres")
        opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IJobService, JobService>();

builder.Services.AddSignalR();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotificationEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(RmqConfig.RmqUri), h =>
        {
            h.Username(RmqConfig.RmqUserName);
            h.Password(RmqConfig.RmqPassword);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.UseMySerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
