using Microsoft.EntityFrameworkCore;
using Quartz;
using QuartzJobManagementDemo.Context;
using QuartzJobManagementDemo.Services.Abstract;
using QuartzJobManagementDemo.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<JobDemoContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("QuartzConnection")));
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IJobService, JobService>();

builder.Services.AddQuartz(cfg =>
{
    cfg.UsePersistentStore(store =>
    {
        store.UseProperties = true;
        store.UseSystemTextJsonSerializer();        
        store.UseSqlServer(builder.Configuration.GetConnectionString("QuartzConnection"));
    });
});
builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<JobDemoContext>();
    dbContext.Database.Migrate();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
