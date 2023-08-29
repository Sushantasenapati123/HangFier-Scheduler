using Hangfier.Domain.Email;
using Hangfier.Irepo.Email;
using Hangfire;
using Hangfire.Logging;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace HangFierSS.Controllers
{
    public class JobController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        
        private readonly IMailService mailService;


      

        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public JobController(IWebHostEnvironment environment, IMailService mailServicee,IRecurringJobManager recurringJobManager, IBackgroundJobClient backgroundJobClient)
        {
            _environment = environment;
            mailService = mailServicee;
            _recurringJobManager = recurringJobManager;
            _backgroundJobClient = backgroundJobClient;
        }
        public IActionResult CreateRecurringJob()
        {
            _backgroundJobClient.Enqueue(() => Background());
            _recurringJobManager.AddOrUpdate("jobId", () => Recurring(), Cron.MinuteInterval(5));
            return Ok();
        }
        public void Recurring()
        {
            SendEmailAsyncFromAPath("Recuring");
            // Get the current date and time
            DateTime currentTime = DateTime.Now;
            // Log the message with current date and time
            Log.Information("Recurring job executed at: {CurrentTime}", currentTime);
          

        }

        public void Background()
        {
            SendEmailAsyncFromAPath("Background");
            Console.WriteLine("Hello HangFire Job!");
        }

        public async Task<JsonResult> SendEmailAsyncFromAPath(string jobtype)
        {
            MailRequest email = new MailRequest();
            //string imagepath = log.GetById(22).Result.image_path.ToString();//prodimage/png1 (5).png

            string attachmentPath = @"C:/Users/sushanta.senapati/Desktop/Sushanta-Kumar-Senapati-Offer-Letter.pdf"; //+ imagepath;
            if (System.IO.File.Exists(attachmentPath))
            {
                using (var memoryStream = new MemoryStream())
                using (var fileStream = new FileStream(attachmentPath, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(memoryStream);

                    memoryStream.Position = 0; // Reset the position to the beginning

                    // Create a byte array from the memory stream
                    byte[] attachmentBytes = memoryStream.ToArray();

                    // Attach the byte array to the email
                    email.AttachmentBytes = attachmentBytes;
                }
                email.Body = jobtype;
                email.Subject = "Success";
                email.ToEmail = "sushantasenapati718@gmail.com";

                await mailService.SendEmailAsyncFromAPath(email);
                Log.Information("Mail Sent At: {CurrentTime}", DateTime.Now);
                return Json(1);
            }

            return Json(1);

        }
    }
}
