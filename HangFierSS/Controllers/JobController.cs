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
            string attachmentPath = @"C:/Users/sushanta.senapati/Desktop/Sushanta-Kumar-Senapati-Offer-Letter.pdf";

            if (System.IO.File.Exists(attachmentPath))
            {
                using (var memoryStream = new MemoryStream())
                using (var fileStream = new FileStream(attachmentPath, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    byte[] attachmentBytes = memoryStream.ToArray();
                    email.AttachmentBytes = attachmentBytes;
                }

                // Create the HTML content for the email body
                string emailBody = @"
    <!DOCTYPE html>
    <html>
    <head>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    </head>
    <body>
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""padding:0px 24px 10px; background-color:white; border-collapse:separate; border:1px solid #e7e7e7; border-bottom:none"">
            <tbody>
                <tr>
                    <td><img src=""https://portal.csm.co.in/kwantify_theme/static/img/bar-line.png"" height=""9px"" width=""380px"" alt=""border-image"" style=""margin:10px 0px""> </td>
                </tr>
                <tr>
                    <td align=""center"" style=""min-width:590px"">
                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""padding:20px 0 0; border-collapse:separate"">
                            <tbody>
                                <tr>
                                    <td valign=""middle"">
                                        <h1 style=""color:#676767; font-weight:400; margin:0px"">Timesheet </h1>
                                    </td>
                                    <td valign=""middle"" align=""right"" width=""200px"">
                                        <img src=""https://portal.csm.co.in/kwantify_theme/static/img/csmlogobtb.png"" alt=""CSM Technologies Pvt. Ltd."" style=""padding:0px; margin:0px; width:150px""> 
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan=""2"" style=""text-align:center"">
                                        <hr width=""100%"" style=""background-color:rgb(204,204,204); border:medium none; clear:both; display:block; font-size:0px; min-height:1px; line-height:0; margin:4px 0px 16px 0px"">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
 <img src=""https://portal.csm.co.in/web/image?model=hr.employee&field=image_medium&id=2685"" alt=""CSM Technologies Pvt. Ltd."" style=""padding:0px; margin:0px; width:150px""> 
                                   
                <tr>
                    <td style=""min-width:590px"">
                        <table border=""0"">
                            <tbody>
                                <tr>
                                    <td>
                                        <div style=""margin-bottom:1em"">

                                            <h5 style=""font-weight:400; margin-bottom:0; font-size:16px; color:#676767""><span style=""color:rgb(22,123,158); font-size:16px; margin-right:2px; font-weight:600"">Dear Sushanta Kumar Senapati, </span></h5>
                                            <p style=""color:#676767; line-height:145%; font-size:16px"">This is a reminder that your timesheet updation is pending for: <b>05-Sep-2023</b> </p>
                                            <p style=""color:#676767; line-height:145%; font-size:16px"">Please log into Kwantify to update your timesheet. </p>
                                            <p style=""color:#676767; line-height:145%; font-size:16px"">Failure to submit your timesheets may result in loss of Earned Leave (EL) or loss of pay. </p>
                                            <p style=""color:#676767; line-height:145%; font-size:16px"">If you have any questions regarding your Timesheet then please contact your Reporting Authority/Project Manager. </p>
                                            <p style=""color:#676767; line-height:170%; font-size:16px"">------------------------------------------------------------------ </p>
                                            <p style=""color:#676767; line-height:170%; font-size:16px"">This is a system generated mail. Please do not reply to this email. </p>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border=""0"" style=""width:100%"">
                            <tbody>
                                <tr>
                                    <td>
                                        <div style=""text-align:center; border-top:1px solid rgb(230,230,230); padding-bottom:20px; padding-top:15px; line-height:125%; font-size:11px; margin:20px 20px 0 20px"">
                                            <p style=""color:rgb(115,115,115); font-size:10px"">© Copyright CSM Technologies, Level-6, OCAC Tower, Bhubaneswar, Odisha, India - 751013 </p>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align=""right"">
                                        <div style=""width:380px; margin:0 20px""><img src=""https://portal.csm.co.in/kwantify_theme/static/img/bar-line2.png"" height=""100%"" width=""100%"" alt=""border-image""> </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border=""0"" style=""width:100%"">
                            <tbody>
                                <tr>
                                    <td>
                                        <div style=""text-align:justify; border-top:1px solid rgb(230,230,230); padding-bottom:10px; padding-top:10px; line-height:125%; font-size:10px; margin:25px 20px 0 20px"">
                                            <p style=""color:rgb(115,115,115); margin:0; font-size:10px"">The information contained in this e-mail message and/or attachments to it may contain confidential or privileged information. If you are not the intended recipient, any dissemination, use, review, distribution, printing or copying of the information contained in this email message and/or attachments to it are strictly prohibited. If you have received this communication in error, please notify us by reply e-mail or telephone and immediately and permanently delete the message and any attachments. Thank you. </p>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </body>
    </html>";


                email.Body = emailBody;
                email.Subject = "Success";
                email.ToEmail = "sushantasenapati718@gmail.com";

                await mailService.SendEmailAsyncFromAPath(email);
                Log.Information("Mail Sent At: {CurrentTime}", DateTime.Now);
                return Json(1);
            }

            return Json(1);
        }

        //public async Task<JsonResult> SendEmailAsyncFromAPath(string jobtype)
        //{
        //    MailRequest email = new MailRequest();
        //    //string imagepath = log.GetById(22).Result.image_path.ToString();//prodimage/png1 (5).png

        //    string attachmentPath = @"C:/Users/sushanta.senapati/Desktop/Sushanta-Kumar-Senapati-Offer-Letter.pdf"; //+ imagepath;
        //    if (System.IO.File.Exists(attachmentPath))
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        using (var fileStream = new FileStream(attachmentPath, FileMode.Open, FileAccess.Read))
        //        {
        //            await fileStream.CopyToAsync(memoryStream);

        //            memoryStream.Position = 0; // Reset the position to the beginning

        //            // Create a byte array from the memory stream
        //            byte[] attachmentBytes = memoryStream.ToArray();

        //            // Attach the byte array to the email
        //            email.AttachmentBytes = attachmentBytes;
        //        }
        //        email.Body = jobtype;
        //        email.Subject = "Success";
        //        email.ToEmail = "rikan862000@gmail.com";

        //        await mailService.SendEmailAsyncFromAPath(email);
        //        Log.Information("Mail Sent At: {CurrentTime}", DateTime.Now);
        //        return Json(1);
        //    }

        //    return Json(1);

        //}
    }
}
