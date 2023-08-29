using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace HangFierSS.Controllers
{
    public class JobController : Controller
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public JobController(IRecurringJobManager recurringJobManager, IBackgroundJobClient backgroundJobClient)
        {
            _recurringJobManager = recurringJobManager;
            _backgroundJobClient = backgroundJobClient;
        }
        public IActionResult CreateRecurringJob()
        {
            _backgroundJobClient.Enqueue(() => Background());
            _recurringJobManager.AddOrUpdate("jobId", () => Recurring(), Cron.Minutely);
            return Ok();
        }
        public void Recurring()
        { // Get the current date and time
            DateTime currentTime = DateTime.Now;
            // Log the message with current date and time
            Log.Information("Recurring job executed at: {CurrentTime}", currentTime);

        }

        public void Background()
        {
            Console.WriteLine("Hello HangFire Job!");
        }
    }
}
