using Microsoft.AspNetCore.Mvc;
using QuartzJobManagementDemo.Shared.Models;
using QuartzJobManagementDemo.Shared.Services.Abstract;
using Serilog;
using System.Diagnostics;

namespace QuartzJobManagementDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IJobService _jobService;

        public HomeController(IMessageService messageService, IJobService jobService)
        {
            _messageService = messageService;
            _jobService = jobService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var messages = _messageService.GetAll();
                var jobs = await _jobService.GetAllAsync();
                var customJobSchedules = await _jobService.GetJobSchedulesAsync();

                return View(new IndexViewModel() { Messages = messages, CustomJobs = jobs, CustomJobSchedules = customJobSchedules });

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                Log.Error(ex, "An error occurred while getting index page.");
                return View(new IndexViewModel() { CustomJobs = new(), Messages = new() });
            }
        }

        public async Task<IActionResult> SaveCustomJob(SaveJobRequestModel saveJobRequestModel)
        {

            try
            {
                await _jobService.AddAsync(new JobViewModel()
                {
                    Name = saveJobRequestModel.JobName,
                    Type = saveJobRequestModel.JobType,
                    Parameters = new() {
                        { "Message", saveJobRequestModel.MessageText ?? string.Empty },
                        { "CreatedBy", saveJobRequestModel.CreatedBy },
                        { "CreatedDate", saveJobRequestModel.CreatedDate.ToString() }
                    }
                });
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
