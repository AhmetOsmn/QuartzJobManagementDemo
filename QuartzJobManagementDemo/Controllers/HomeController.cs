using Microsoft.AspNetCore.Mvc;
using QuartzJobManagementDemo.Models;
using QuartzJobManagementDemo.Services.Abstract;
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

        public IActionResult Index()
        {
            try
            {
                var messages = _messageService.GetAll();
                var jobs = _jobService.GetAll();
                var customJobSchedules = _jobService.GetJobSchedules();

                return View(new IndexViewModel() { Messages = messages, CustomJobs = jobs, CustomJobSchedules = customJobSchedules });

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new IndexViewModel() { CustomJobs = new(), Messages = new() });
            }
        }

        public IActionResult SaveCustomJob(SaveJobRequestModel saveJobRequestModel)
        {

            try
            {
                _jobService.Add(new JobViewModel()
                {
                    Name = saveJobRequestModel.JobName,
                    Type = saveJobRequestModel.JobType,                    
                });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ScheduleCustomJob(string jobId, string cronExpression)
        {
            try
            {
                _jobService.Schedule(jobId, cronExpression);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteJob(string id)
        {
            try
            {
                _jobService.Delete(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
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
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteJobSchedule(string id)
        {
            try
            {
                _jobService.DeleteJobSchedule(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
