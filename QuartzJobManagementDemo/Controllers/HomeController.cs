using Microsoft.AspNetCore.Mvc;
using QuartzJobManagementDemo.Models;
using QuartzJobManagementDemo.Services.Abstract;
using Serilog;
using System.Diagnostics;

namespace QuartzJobManagementDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IJobService _jobService;
        private readonly IConfiguration _configuration;

        public HomeController(IMessageService messageService, IJobService jobService, IConfiguration configuration)
        {
            _messageService = messageService;
            _jobService = jobService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var messages = _messageService.GetAll();
                var jobs = await _jobService.GetAllAsync();
                var customJobSchedules = await _jobService.GetJobSchedulesAsync();

                return View(new IndexViewModel() { Messages = messages, CustomJobs = jobs?.Data?.ToList() ?? [], CustomJobSchedules = customJobSchedules?.Data?.ToList() ?? [] });

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Log.Error(ex, "An error occurred while getting index page.");
                return View(new IndexViewModel() { CustomJobs = [], Messages = [] });
            }
        }

        public async Task<IActionResult> SaveCustomJob(SaveJobRequestModel saveJobRequestModel)
        {

            try
            {
                var database = _configuration["Database"] ?? throw new InvalidOperationException("Database string not found.");

                var connectionString = database == "Postgres" ? _configuration.GetConnectionString("Postgres") : _configuration.GetConnectionString("SqlServer");

                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidOperationException($"{database} Connection string not found.");

                await _jobService.AddAsync(saveJobRequestModel.JobName, new() {
                        { "Message", saveJobRequestModel.MessageText ?? string.Empty },
                        { "CreatedBy", saveJobRequestModel.CreatedBy },
                        { "ConnectionString", connectionString},
                        { "Database",  database},
                        { "CreatedDate", saveJobRequestModel.CreatedDate.ToString() }
                    }, saveJobRequestModel.JobType);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Log.Error(ex, "An error occurred while saving job. {JobName}", saveJobRequestModel.JobName);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ScheduleCustomJob(string jobName, string cronExpression)
        {
            try
            {
                await _jobService.ScheduleAsync(jobName, cronExpression);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Log.Error(ex, "An error occurred while scheduling job. {JobName}", jobName);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteJob(string name)
        {
            try
            {
                _jobService.DeleteAsync(name);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Log.Error(ex, "An error occurred while deleting job. {JobName}", name);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteMessage(int id)
        {
            try
            {
                _messageService.Delete(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Log.Error(ex, "An error occurred while deleting message. {MessageId}", id);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteMessages()
        {
            try
            {
                _messageService.DeleteAll();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Log.Error(ex, "An error occurred while deleting messages.");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteJobSchedule(string name)
        {
            try
            {
                await _jobService.DeleteJobScheduleAsync(name);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Log.Error(ex, "An error occurred while deleting job schedule. {JobName}", name);
            }
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            Log.Error("An error occurred. {RequestId}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
